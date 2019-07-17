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

        public RecordSearch CurrentRecordSearch { get; set; }

        public string ConnectionString { get; set; }

        //Constructor
        public RecordSearchService(IClientService clientService, IPersonService personService, IAddressService addressService)
        {
            _cs = clientService;
            _ps = personService;
            _as = addressService;
            SetConnectionString();
        }

        //Methods
        public void SetConnectionString()
        {
            //TODO Have this pull from a setting.
            var dir = Directory.GetCurrentDirectory();
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir + @"\Resources\RS_Backend.accdb";
        }

        public RecordSearch GetRecordSearchByID(int ID, bool loadAsCurrentSearch = true)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = @"SELECT ID, RSPrefix, RSYear, RSEnumeration, RSSuffix," +
                        "DateReceived, DateEntered, DateOfResponse, DateBilled, DatePaid, LastUpdated, " +
                        "RequestorID, ClientID, MailingAddressID, IsMailingAddressSameAsBilling, BillingAddressID, " +
                        "ProjectName, RecordSearchType, IsSpecialCase, SpecialCaseDetails, " +
                        "MainCounty, PLSS, Acres, LinearMiles, " +
                        "AreResourcesInProject, Recommendation, IsReportReceived, " +
                        "FeeVersion, FeeID, DiscretionaryAdjustment, AdjustmentExplanation, " +
                        "ProjectNumber, InvoiceNumber, CheckName, CheckNumber, EncryptionPassword " +
                        "FROM tblRecordSearches " +
                        "WHERE ID = " + ID;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    int index = 0;
                    RecordSearch returnValue = new RecordSearch()
                    {
                        ID = reader.GetInt32Safe(index++), //0
                        RSTypePrefix = reader.GetStringSafe(index++),
                        RSYear = reader.GetStringSafe(index++),
                        RSEnumeration = reader.GetInt32Safe(index++),
                        RSSuffix = reader.GetStringSafe(index++), 
                        DateReceived = reader.GetDateTimeSafe(index++), //5
                        DateEntered = reader.GetDateTimeSafe(index++),
                        DateOfResponse = reader.GetDateTimeSafe(index++),
                        DateBilled = reader.GetDateTimeSafe(index++),
                        DatePaid = reader.GetDateTimeSafe(index++),
                        LastUpdated = reader.GetDateTimeSafe(index++),  //10
                        RequestorID = reader.GetInt32Safe(index),
                        Requestor = _ps.GetPersonByID(reader.GetInt32Safe(index++), false),
                        ClientID = reader.GetInt32Safe(index),
                        ClientModel = _cs.GetClientByID(reader.GetInt32Safe(index++), false),
                        MailingAddress = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                        IsMailingSameAsBilling = reader.GetBooleanSafe(index++), //15
                        BillingAddress = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                        ProjectName = reader.GetStringSafe(index++),
                        RSType = reader.GetStringSafe(index++),
                        Special = reader.GetBooleanSafe(index++),
                        SpecialDetails = reader.GetStringSafe(index++), //20
                        MainCounty = reader.GetStringSafe(index++),
                        //AdditionalCounties = GetAdditionalCounties(reader.GetInt32Safe(index++)),
                        PLSS = reader.GetStringSafe(index++),
                        Acres = reader.GetInt32Safe(index++),
                        LinearMiles = reader.GetInt32Safe(index++), //25
                        AreResourcesInProject = reader.GetBooleanSafe(index++),
                        Recommendation = reader.GetStringSafe(index++),
                        IsReportReceived = reader.GetBooleanSafe(index++),
                        FeeVersion = reader.GetStringSafe(index) ?? Properties.Settings.Default.FeeType,
                        Fee = GetFeeData(reader.GetStringSpecial(index++), reader.GetInt32Safe(index++)), //30
                        DiscretionaryAdjustment = reader.GetDecimalSafe(index++),
                        AdjustmentExplanation = reader.GetStringSafe(index++),
                        ProjectNumber = reader.GetStringSafe(index++), 
                        InvoiceNumber = reader.GetStringSafe(index++),
                        CheckName = reader.GetStringSafe(index++), //35
                        CheckNumber = reader.GetStringSafe(index++),
                        EncryptionPassword = reader.GetStringSafe(index++)
                    };

                    // TODO Load Fee info
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

        public List<RecordSearch> GetAllPartialRecordSearches()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = @"Select ID, RSPrefix, RSYear, RSEnumeration, RSSuffix, ProjectName, LastUpdated from tblRecordSearches";
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    List<RecordSearch> returnCollection = new List<RecordSearch>();

                    while (reader.Read())
                    {
                        int index = 0;
                        RecordSearch returnValue = new RecordSearch()
                        {
                            ID = reader.GetInt32Safe(index++),
                            RSTypePrefix = reader.GetStringSafe(index++),
                            RSYear = reader.GetStringSafe(index++),
                            RSEnumeration = reader.GetInt32Safe(index++),
                            RSSuffix = reader.GetStringSafe(index++),
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
                    sqlCommand.CommandText = @"Select ID, RSPrefix, RSYear, RSEnumeration, RSSuffix, ProjectName, LastUpdated, RequestorID from tblRecordSearches " + criteria;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    List<RecordSearch> returnCollection = new List<RecordSearch>();

                    while (reader.Read())
                    {
                        int index = 0;
                        RecordSearch returnValue = new RecordSearch()
                        {
                            ID = reader.GetInt32Safe(index++),
                            RSTypePrefix = reader.GetStringSafe(index++),
                            RSYear = reader.GetStringSafe(index++),
                            RSEnumeration = reader.GetInt32Safe(index++),
                            RSSuffix = reader.GetStringSafe(index++),
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
                    sqlCommand.CommandText = "INSERT INTO tblRecordSearches (RSPrefix, RSYear, RSEnumeration, RSSuffix, ProjectName, DateEntered, LastUpdated) " +
                        "VALUES (?,?,?,?,?,?,?)";
                    sqlCommand.Parameters.AddWithValue("RSPrefix", array[0]);
                    sqlCommand.Parameters.AddWithValue("RSYear", array[1]);
                    sqlCommand.Parameters.AddWithValue("RSEnumeration", array[2]);
                    sqlCommand.Parameters.AddWithValue("RSSuffix", array[3] ?? DBNull.Value);
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
            return new Fee(version);
        }

        public int GetNextEnumeration(string prefix, string year)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT MAX(RSEnumeration) FROM tblRecordSearches WHERE RSPrefix = @prefix AND RSYear = @year";
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
                        sqlCommand.CommandText = "SELECT ID FROM tblRecordSearches WHERE RSPrefix = @prefix AND RSYear = @year AND RSEnumeration = @enumeration";
                        sqlCommand.Parameters.Add(new OleDbParameter("@prefix", prefix));
                        sqlCommand.Parameters.Add(new OleDbParameter("@year", year));
                        sqlCommand.Parameters.Add(new OleDbParameter("@enumeration", enumeration));
                    }
                    else
                    {
                        sqlCommand.CommandText = "SELECT ID FROM tblRecordSearches WHERE RSPrefix = @prefix AND RSYear = @year AND RSEnumeration = @enumeration AND RSSuffix = @suffix";
                        sqlCommand.Parameters.Add(new OleDbParameter("@prefix", prefix));
                        sqlCommand.Parameters.Add(new OleDbParameter("@year", year));
                        sqlCommand.Parameters.Add(new OleDbParameter("@enumeration", enumeration));
                        sqlCommand.Parameters.Add(new OleDbParameter("@suffix", suffix));
                    }
                    
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    if(reader.HasRows)
                        return false;
                    return true;
                }
            }
        }
    }
}
