using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
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
                        "MainCounty, AdditionalCounties, PLSS, Acres, LinearMiles, " +
                        "AreResourcesInProject, Recommendation, IsReportReceived, Processor, EncryptionPassword, " +
                        "FeeVersion, FeeID, TotalCost, " +
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
                        AdditionalCounties = ParseAdditionalCounties(reader.GetStringSafe(index++)),
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
                        ProjectNumber = reader.GetStringSafe(index++),
                        InvoiceNumber = reader.GetStringSafe(index++),
                        CheckName = reader.GetStringSafe(index++),
                        CheckNumber = reader.GetStringSafe(index++),
                        IsPrePaid = reader.GetBooleanSafe(index++),
                        IsSelected = reader.GetBooleanSafe(index++),
                        Notes = reader.GetStringSafe(index++),
                    };

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
                        "MainCounty, AdditionalCounties, PLSS, Acres, LinearMiles, " +
                        "AreResourcesInProject, Recommendation, IsReportReceived, Processor, EncryptionPassword, " +
                        "FeeVersion, FeeID, TotalCost, " +
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
                            AdditionalCounties = ParseAdditionalCounties(reader.GetStringSafe(index++)),
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
                    sqlCommand.CommandText = @"SELECT ID, ICPrefix, ICYear, ICEnumeration, ICSuffix, ProjectName, Status, LastUpdated FROM tblRecordSearches";
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
                            Status = reader.GetStringSafe(index++),
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
                    sqlCommand.CommandText = @"SELECT ID, ICPrefix, ICYear, ICEnumeration, ICSuffix, ProjectName, Status, LastUpdated FROM tblRecordSearches " + criteria;
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
                            Status = reader.GetStringSafe(index++),
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

        public void UpdateRecordSearch(RecordSearch rs)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    rs.MailingAddress.AddressID = _as.UpdateAddress(rs.MailingAddress);
                    rs.BillingAddress.AddressID = _as.UpdateAddress(rs.BillingAddress);
                    rs.FeeID = _fs.UpdateFee(rs.Fee);

                    sqlCommand.CommandText = "UPDATE tblRecordSearches SET " +
                        "ICPrefix = @ICPrefix, ICYear = @ICYear, ICEnumeration = @ICEnumeration, ICSuffix = ?," +
                        "DateReceived = @DateReceived, DateEntered = @DateEntered, DateOfResponse = @DateOfResponse, DateBilled = @DateBilled, DatePaid = @DatePaid, LastUpdated = @LastUpdated, " +
                        "RequestorID = @RequestorID, AdditionalRequestors = @AdditionalRequestors, ClientID = @ClientID, MailingAddressID = @MailingAddressID, IsMailingAddressSameAsBilling = @IsMailingAddressSameAsBilling, BillingAddressID = @BillingAddressID, " +
                        "ProjectName = @ProjectName, RecordSearchType = @RecordSearchType, Status = @Status, SpecialCaseDetails = @SpecialCaseDetails, " +
                        "MainCounty = @MainCounty, AdditionalCounties = @AdditionalCountiesID, PLSS = @PLSS, Acres = @Acres, LinearMiles = @LinearMiles, " +
                        "AreResourcesInProject = @AreResourcesInProject, Recommendation = @Recommendation, IsReportReceived = @IsReportReceived, Processor = @Processor, EncryptionPassword = @EncryptionPassword, " +
                        "FeeVersion = @FeeVersion, FeeID = @FeeID, TotalCost = @TotalCost, " +
                        "ProjectNumber = @ProjectNumber, InvoiceNumber = @InvoiceNumber, CheckName = @CheckName, CheckNumber = @CheckNumber, IsPrePaid = @IsPrePaid, IsSelected = @IsSelected, Notes = @Notes " +
                        "WHERE ID = ?";
                    sqlCommand.Parameters.AddWithValue("@ICPrefix", rs.ICTypePrefix ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@ICYear", rs.ICYear ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@ICEnumeration", rs.ICEnumeration);
                    sqlCommand.Parameters.AddWithValue("@ICSuffix", rs.ICSuffix ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@DateReceived", rs.DateReceived ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@DateEntered", rs.DateEntered ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@DateOfResponse", rs.DateOfResponse ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@DateBilled", rs.DateBilled ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@DatePaid", rs.DatePaid ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@LastUpdated", rs.LastUpdated ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@RequestorID", rs.RequestorID); //??
                    sqlCommand.Parameters.AddWithValue("@AdditionalRequestors", rs.AdditionalRequestors ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@ClientID", rs.ClientID); //??
                    sqlCommand.Parameters.AddWithValue("@MailingAddressID", rs.MailingAddress.AddressID);
                    sqlCommand.Parameters.AddWithValue("@IsMailingAddressSameAsBilling", rs.IsMailingSameAsBilling);
                    sqlCommand.Parameters.AddWithValue("@BillingAddressID", rs.BillingAddress.AddressID);

                    sqlCommand.Parameters.AddWithValue("@ProjectName", rs.ProjectName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@RecordSearchType", rs.RSType ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@Status", rs.Status ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@SpecialCaseDetails", rs.SpecialDetails ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@MainCounty", rs.MainCounty ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@AdditionalCounties", WriteAdditionalCounties(rs.AdditionalCounties)); // TODO figure out additional counties field
                    sqlCommand.Parameters.AddWithValue("@PLSS", rs.PLSS ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@Acres", rs.Acres);
                    sqlCommand.Parameters.AddWithValue("@LinearMiles", rs.LinearMiles);

                    sqlCommand.Parameters.AddWithValue("@AreResourcesInProject", rs.AreResourcesInProject);
                    sqlCommand.Parameters.AddWithValue("@Recommendation", rs.Recommendation ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@IsReportReceived", rs.IsReportReceived);
                    sqlCommand.Parameters.AddWithValue("@Processor", rs.Processor ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@EncryptionPassword", rs.EncryptionPassword ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@FeeVersion", rs.FeeVersion ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@FeeID", rs.FeeID); //??
                    sqlCommand.Parameters.AddWithValue("@TotalCost", rs.TotalFee);
                    
                    sqlCommand.Parameters.AddWithValue("@ProjectNumber", rs.ProjectNumber ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@InvoiceNumber", rs.InvoiceNumber ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@CheckName", rs.CheckName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@CheckNumber", rs.CheckNumber ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@IsPrePaid", rs.IsPrePaid);
                    sqlCommand.Parameters.AddWithValue("@IsSelected", rs.IsSelected);
                    sqlCommand.Parameters.AddWithValue("@Notes", rs.Notes ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("ID", rs.ID);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        private ObservableCollection<County> ParseAdditionalCounties(string additionalCounties)
        {
            ObservableCollection<County> returnCollection = new ObservableCollection<County>();
            if (!string.IsNullOrWhiteSpace(additionalCounties))
            {
                foreach (string individualCounty in additionalCounties.Split(','))
                {
                    returnCollection.Add(Counties.Values.First(c => c.Name == individualCounty));
                }
            }
            return returnCollection;
        }

        private string WriteAdditionalCounties(ObservableCollection<County> additionalCounties)
        {
            string returnValue = "";
            foreach (County county in additionalCounties)
            {
                returnValue += county.Name + ",";
            }
            return returnValue.TrimEnd(',');
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
