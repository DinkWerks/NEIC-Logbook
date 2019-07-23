using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using Tracker.Core.Extensions;
using Tracker.Core.Models;
using Tracker.Core.Models.Fees;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Services
{
    public class RecordSearchService : IRecordSearchService
    {
        private IClientService _cs;
        private IPersonService _ps;
        private IAddressService _as;
        private IFeeService _fs;

        public RecordSearch CurrentRecordSearch { get; set; }

        public string ConnectionString { get; set; }

        //Constructor
        public RecordSearchService(IClientService clientService, IPersonService personService, IAddressService addressService, IFeeService feeService)
        {
            _cs = clientService;
            _ps = personService;
            _as = addressService;
            _fs = feeService;
            SetConnectionString();
        }

        //Methods
        public void SetConnectionString()
        {
            //TODO Have this pull from a setting.
            var dir = Directory.GetCurrentDirectory();
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir + @"\Resources\RS_Backend.accdb";
        }

        public RecordSearch GetRecordSearchByID(int id, bool loadAsCurrentSearch = true)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, ICPrefix, ICYear, ICEnumeration, ICSuffix, " +
                        "DateReceived, DateEntered, DateOfResponse, DateBilled, DatePaid, LastUpdated, " +
                        "RequestorID, AdditionalRequestors, ClientID, MailingAddressID, IsMailingAddressSameAsBilling, BillingAddressID, " +
                        "ProjectName, RecordSearchType, Status, SpecialCaseDetails, " +
                        "MainCounty, AdditionalCountiesID, PLSS, Acres, LinearMiles, " +
                        "AreResourcesInProject, Recommendation, IsReportReceived, Processor, EncryptionPassword, " +
                        "FeeVersion, FeeID, TotalCost, DiscretionaryAdjustment, AdjustmentExplanation, " +
                        "ProjectNumber, InvoiceNumber, CheckName, CheckNumber, IsPrePaid, IsSelected, Notes " +
                        "FROM tblRecordSearches WHERE ID = " + id;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    int index = 0;
                    RecordSearch returnValue = new RecordSearch()
                    {
                        ID = reader.GetInt32Safe(index++), //0
                        ICTypePrefix = reader.GetStringSafe(index++),
                        ICYear = reader.GetStringSafe(index++),
                        ICEnumeration = reader.GetInt32Safe(index++),
                        ICSuffix = reader.GetStringSafe(index++),
                        DateReceived = reader.GetDateTimeSafe(index++), //5
                        DateEntered = reader.GetDateTimeSafe(index++),
                        DateOfResponse = reader.GetDateTimeSafe(index++),
                        DateBilled = reader.GetDateTimeSafe(index++),
                        DatePaid = reader.GetDateTimeSafe(index++),
                        LastUpdated = reader.GetDateTimeSafe(index++), //10
                        RequestorID = reader.GetInt32Safe(index),
                        Requestor = _ps.GetPersonByID(reader.GetInt32Safe(index++)),
                        AdditionalRequestors = reader.GetStringSafe(index++),
                        ClientID = reader.GetInt32Safe(index),
                        ClientModel = _cs.GetClientByID(reader.GetInt32Safe(index++)), //15
                        MailingAddress = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                        IsMailingSameAsBilling = reader.GetBooleanSafe(index++),
                        BillingAddress = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                        ProjectName = reader.GetStringSafe(index++),
                        RSType = reader.GetStringSafe(index++), //20
                        Status = reader.GetStringSafe(index++),
                        SpecialDetails = reader.GetStringSafe(index++),
                        MainCounty = reader.GetStringSafe(index++),
                        AdditionalCounties = GetAdditionalCounties(reader.GetInt32Safe(index++)),
                        PLSS = reader.GetStringSafe(index++), //25
                        Acres = reader.GetInt32Safe(index++),
                        LinearMiles = reader.GetInt32Safe(index++),
                        AreResourcesInProject = reader.GetBooleanSafe(index++),
                        Recommendation = reader.GetStringSafe(index++),
                        IsReportReceived = reader.GetBooleanSafe(index++), //30
                        Processor = reader.GetStringSafe(index++),
                        EncryptionPassword = reader.GetStringSafe(index++),
                        FeeVersion = reader.GetStringSafe(index++),
                        FeeID = reader.GetInt32Safe(index),
                        Fee = GetFeeData("v2017", reader.GetInt32Safe(index++)),
                        TotalFee = reader.GetDecimalSafe(index++),
                        DiscretionaryAdjustment = reader.GetDecimalSafe(index++),
                        AdjustmentExplanation = reader.GetStringSafe(index++),
                        ProjectNumber = reader.GetStringSafe(index++),
                        InvoiceNumber = reader.GetStringSafe(index++),
                        CheckName = reader.GetStringSafe(index++),
                        CheckNumber = reader.GetStringSafe(index++),
                        IsPrePaid = reader.GetBooleanSafe(index++),
                        IsSelected = reader.GetBooleanSafe(index++),
                        Notes = reader.GetStringSafe(index++),
                    };

                    // TODO Load Fee info
                    returnValue.Fee.Adjustment = returnValue.DiscretionaryAdjustment;
                    if (returnValue.IsMailingSameAsBilling)
                        returnValue.BillingAddress = returnValue.MailingAddress;
                    if (loadAsCurrentSearch)
                    {
                        CurrentRecordSearch = returnValue;
                    }
                    return returnValue;
                }
            }
        }

        public List<RecordSearch> GetRecordSearchesByCriteria(string criteria)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, ICPrefix, ICYear, ICEnumeration, ICSuffix, " +
                        "DateReceived, DateEntered, DateOfResponse, DateBilled, DatePaid, LastUpdated, " +
                        "RequestorID, AdditionalRequestors, ClientID, MailingAddressID, IsMailingAddressSameAsBilling, BillingAddressID, " +
                        "ProjectName, RecordSearchType, Status, SpecialCaseDetails, " +
                        "MainCounty, AdditionalCountiesID, PLSS, Acres, LinearMiles, " +
                        "AreResourcesInProject, Recommendation, IsReportReceived, Processor, EncryptionPassword, " +
                        "FeeVersion, FeeID, TotalCost, DiscretionaryAdjustment, AdjustmentExplanation, " +
                        "ProjectNumber, InvoiceNumber, CheckName, CheckNumber, IsPrePaid, IsSelected, Notes " +
                        "FROM tblRecordSearches " + criteria;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    List<RecordSearch> returnCollection = new List<RecordSearch>();

                    while (reader.Read())
                    {
                        int index = 0;
                        RecordSearch returnValue = new RecordSearch()
                        {
                            ID = reader.GetInt32Safe(index++), //0
                            ICTypePrefix = reader.GetStringSafe(index++),
                            ICYear = reader.GetStringSafe(index++),
                            ICEnumeration = reader.GetInt32Safe(index++),
                            ICSuffix = reader.GetStringSafe(index++),
                            DateReceived = reader.GetDateTimeSafe(index++), //5
                            DateEntered = reader.GetDateTimeSafe(index++),
                            DateOfResponse = reader.GetDateTimeSafe(index++),
                            DateBilled = reader.GetDateTimeSafe(index++),
                            DatePaid = reader.GetDateTimeSafe(index++),
                            LastUpdated = reader.GetDateTimeSafe(index++), //10
                            RequestorID = reader.GetInt32Safe(index),
                            Requestor = _ps.GetPersonByID(reader.GetInt32Safe(index++)),
                            AdditionalRequestors = reader.GetStringSafe(index++),
                            ClientID = reader.GetInt32Safe(index),
                            ClientModel = _cs.GetClientByID(reader.GetInt32Safe(index++)), //15
                            MailingAddress = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                            IsMailingSameAsBilling = reader.GetBooleanSafe(index++),
                            BillingAddress = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                            ProjectName = reader.GetStringSafe(index++),
                            RSType = reader.GetStringSafe(index++), //20
                            Status = reader.GetStringSafe(index++),
                            SpecialDetails = reader.GetStringSafe(index++),
                            MainCounty = reader.GetStringSafe(index++),
                            AdditionalCounties = GetAdditionalCounties(reader.GetInt32Safe(index++)),
                            PLSS = reader.GetStringSafe(index++), //25
                            Acres = reader.GetInt32Safe(index++),
                            LinearMiles = reader.GetInt32Safe(index++),
                            AreResourcesInProject = reader.GetBooleanSafe(index++),
                            Recommendation = reader.GetStringSafe(index++),
                            IsReportReceived = reader.GetBooleanSafe(index++), //30
                            Processor = reader.GetStringSafe(index++),
                            EncryptionPassword = reader.GetStringSafe(index++),
                            FeeVersion = reader.GetStringSafe(index++),
                            FeeID = reader.GetInt32Safe(index),
                            Fee = GetFeeData("v2017", reader.GetInt32Safe(index++)),
                            TotalFee = reader.GetDecimalSafe(index++),
                            DiscretionaryAdjustment = reader.GetDecimalSafe(index++),
                            AdjustmentExplanation = reader.GetStringSafe(index++),
                            ProjectNumber = reader.GetStringSafe(index++),
                            InvoiceNumber = reader.GetStringSafe(index++),
                            CheckName = reader.GetStringSafe(index++),
                            CheckNumber = reader.GetStringSafe(index++),
                            IsPrePaid = reader.GetBooleanSafe(index++),
                            IsSelected = reader.GetBooleanSafe(index++),
                            Notes = reader.GetStringSafe(index++),
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public List<RecordSearch> GetAllPartialRecordSearches()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = @"SELECT ID, ICPrefix, ICYear, ICEnumeration, ICSuffix, ProjectName, LastUpdated FROM tblRecordSearches";
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    List<RecordSearch> returnCollection = new List<RecordSearch>();

                    while (reader.Read())
                    {
                        int index = 0;
                        RecordSearch returnValue = new RecordSearch()
                        {
                            ID = reader.GetInt32Safe(index++),
                            ICTypePrefix = reader.GetStringSafe(index++),
                            ICYear = reader.GetStringSafe(index++),
                            ICEnumeration = reader.GetInt32Safe(index++),
                            ICSuffix = reader.GetStringSafe(index++),
                            ProjectName = reader.GetStringSafe(index++),
                            LastUpdated = reader.GetDateTimeSafe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public List<RecordSearch> GetPartialRecordSearchesByCriteria(string criteria)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = @"SELECT ID, ICPrefix, ICYear, ICEnumeration, ICSuffix, ProjectName, LastUpdated, RequestorID FROM tblRecordSearches " + criteria;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    List<RecordSearch> returnCollection = new List<RecordSearch>();

                    while (reader.Read())
                    {
                        int index = 0;
                        RecordSearch returnValue = new RecordSearch()
                        {
                            ID = reader.GetInt32Safe(index++),
                            ICTypePrefix = reader.GetStringSafe(index++),
                            ICYear = reader.GetStringSafe(index++),
                            ICEnumeration = reader.GetInt32Safe(index++),
                            ICSuffix = reader.GetStringSafe(index++),
                            ProjectName = reader.GetStringSafe(index++),
                            LastUpdated = reader.GetDateTimeSafe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public int AddNewRecordSearch(object[] array)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "INSERT INTO tblRecordSearches (ICPrefix, ICYear, ICEnumeration, ICSuffix, ProjectName, DateEntered, LastUpdated) " +
                        "VALUES (?,?,?,?,?,?,?)";
                    sqlCommand.Parameters.AddWithValue("ICPrefix", array[0]);
                    sqlCommand.Parameters.AddWithValue("ICYear", array[1]);
                    sqlCommand.Parameters.AddWithValue("ICEnumeration", array[2]);
                    sqlCommand.Parameters.AddWithValue("ICSuffix", array[3] ?? DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("ProjectName", array[4]);
                    sqlCommand.Parameters.AddWithValue("DateEntered", array[5]);
                    sqlCommand.Parameters.AddWithValue("LastUpdated", array[6]);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "Select @@identity";
                    return (int)sqlCommand.ExecuteScalar();
                }
            }
        }

        public ObservableCollection<County> GetAdditionalCounties(int id)
        {
            return null;
        }

        public Fee GetFeeData(string version, int id)
        {
            Fee newFee = new Fee(version)
            {
                ID = id
            };
            _fs.GetFeeData(newFee);
            return newFee;
        }

        public int GetNextEnumeration(string prefix, string year)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT MAX(ICEnumeration) FROM tblRecordSearches WHERE ICPrefix = @prefix AND ICYear = @year";
                    sqlCommand.Parameters.Add(new OleDbParameter("@prefix", prefix));
                    sqlCommand.Parameters.Add(new OleDbParameter("@year", year));

                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    int value = reader.GetInt32Safe(0);
                    return value + 1;
                }
            }
        }

        public bool ConfirmDistinct(string prefix, string year, int enumeration, string suffix)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    if (string.IsNullOrWhiteSpace(suffix))
                    {
                        sqlCommand.CommandText = "SELECT ID FROM tblRecordSearches WHERE ICPrefix = @prefix AND ICYear = @year AND ICEnumeration = @enumeration";
                        sqlCommand.Parameters.Add(new OleDbParameter("@prefix", prefix));
                        sqlCommand.Parameters.Add(new OleDbParameter("@year", year));
                        sqlCommand.Parameters.Add(new OleDbParameter("@enumeration", enumeration));
                    }
                    else
                    {
                        sqlCommand.CommandText = "SELECT ID FROM tblRecordSearches WHERE ICPrefix = @prefix AND ICYear = @year AND ICEnumeration = @enumeration AND ICSuffix = @suffix";
                        sqlCommand.Parameters.Add(new OleDbParameter("@prefix", prefix));
                        sqlCommand.Parameters.Add(new OleDbParameter("@year", year));
                        sqlCommand.Parameters.Add(new OleDbParameter("@enumeration", enumeration));
                        sqlCommand.Parameters.Add(new OleDbParameter("@suffix", suffix));
                    }

                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows)
                        return false;
                    return true;
                }
            }
        }
    }
}
