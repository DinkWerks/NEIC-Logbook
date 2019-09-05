using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using Tracker.Core.Extensions;
using Tracker.Core.Models;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Services
{
    public class ClientService : IClientService
    {
        private IAddressService _as;

        public Client CurrentClient { get; set; }
        public List<Client> CompleteClientList { get; set; }
        public string ConnectionString { get; set; }

        public ClientService(IAddressService addressService)
        {
            _as = addressService;
            SetConnectionString();
            CompleteClientList = GetAllPartialClients();
        }

        public void SetConnectionString()
        {
            var dir = Settings.Settings.Instance.DatabaseAddress;
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir;
        }

        public Client GetClientByID(int id, bool loadAsCurrentClient = true)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = @"SELECT ID, OldPEID, NewPEID, ClientName, OfficeName, Phone, Email, Website,
                    Standing, AddressID, Notes FROM tblClients WHERE ID = ?";
                    sqlCommand.Parameters.AddWithValue("ID", id);
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    if (!reader.HasRows)
                        return new Client();

                    int index = 0;
                    Client returnValue = new Client()
                    {
                        ID = reader.GetInt32Safe(index++),
                        OldPEID = reader.GetStringSafe(index++),
                        NewPEID = reader.GetStringSafe(index++),
                        ClientName = reader.GetStringSafe(index++),
                        OfficeName = reader.GetStringSafe(index++),
                        Phone = reader.GetStringSafe(index++),
                        Email = reader.GetStringSafe(index++),
                        Website = reader.GetStringSafe(index++),
                        Standing = ParseStanding(reader.GetStringSafe(index++)),
                        AddressID = reader.GetInt32Safe(index),
                        AddressModel = _as.GetAddressByID(reader.GetInt32Safe(index++)),
                        Notes = reader.GetStringSafe(index++)
                    };

                    if (loadAsCurrentClient)
                    {
                        CurrentClient = returnValue;
                    }
                    return returnValue;
                }
            }
        }

        public List<Client> GetAllPartialClients()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = @"SELECT ID, OldPEID, NewPEID, ClientName, OfficeName, Standing, AddressID FROM tblClients";
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    List<Client> returnCollection = new List<Client>();

                    while (reader.Read())
                    {
                        int index = 0;
                        Client returnValue = new Client()
                        {
                            ID = reader.GetInt32Safe(index++),
                            OldPEID = reader.GetStringSafe(index++),
                            NewPEID = reader.GetStringSafe(index++),
                            ClientName = reader.GetStringSafe(index++),
                            OfficeName = reader.GetStringSafe(index++),
                            Standing = ParseStanding(reader.GetStringSafe(index++)),
                            AddressID = reader.GetInt32Safe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public int AddNewClient(Client newClient)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "INSERT INTO tblClients (ClientName, OfficeName, Standing) " +
                        "VALUES (?,?,?)";
                    sqlCommand.Parameters.AddWithValue("ClientName", newClient.ClientName);
                    sqlCommand.Parameters.AddWithValue("OfficeName", newClient.OfficeName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("Standing", "Good Standing");

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "Select @@identity";
                    int newID = (int)sqlCommand.ExecuteScalar();

                    CompleteClientList = GetAllPartialClients();
                    return newID;
                }
            }
        }

        public void UpdateClientInformation(Client c)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE tblClients SET " +
                        "OldPEID = @oldpeid, NewPEID = @newpeid, ClientName = @clientname, OfficeName = @officename, " +
                        "Phone = @phone, Email = @email, Website = @website, " +
                        "Standing = @standing, AddressID = @addressID, Notes = @notes " +
                        "WHERE ID = @id";
                    sqlCommand.Parameters.AddWithValue("@oldpeid", c.OldPEID ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@newpeid", c.NewPEID ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@clientname", c.ClientName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@officename", c.OfficeName ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@phone", c.Phone ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@email", c.Email ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@website", c.Website ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@standing", c.Standing.Name ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@addressID", c.AddressID);
                    sqlCommand.Parameters.AddWithValue("@notes", c.Notes ?? Convert.DBNull);

                    sqlCommand.Parameters.AddWithValue("@id", c.ID);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            CompleteClientList = GetAllPartialClients();
        }

        public void RemoveClient(int id, int addressID)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "DELETE FROM tblClients WHERE ID = " + id;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();

                    _as.RemoveAddress(addressID);
                }
            }
        }

        public bool ConfirmDistinct(string clientName, string officeName)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    if (string.IsNullOrWhiteSpace(officeName))
                    {
                        sqlCommand.CommandText = "SELECT ID FROM tblClients WHERE ClientName = @clientName";
                        sqlCommand.Parameters.AddWithValue("@clientName", clientName ?? Convert.DBNull);
                    }
                    else
                    {
                        sqlCommand.CommandText = "SELECT ID FROM tblClients WHERE ClientName = @clientName AND OfficeName = @officeName";
                        sqlCommand.Parameters.AddWithValue("@clientName", clientName ?? Convert.DBNull);
                        sqlCommand.Parameters.AddWithValue("@officeName", officeName ?? Convert.DBNull);
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

        private ClientStanding ParseStanding(string standingName)
        {
            if (!string.IsNullOrWhiteSpace(standingName))
            {
                return ClientStandings.Values.First(s => s.Name == standingName);
            }
            else
                return ClientStandings.GoodStanding;
        }
    }
}
