using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using Tracker.Core.Extensions;
using Tracker.Core.Models;

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
            //TODO Have this pull from a setting.
            var dir = Directory.GetCurrentDirectory();
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir + @"\Resources\RS_Backend.accdb";
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
                        Standing = reader.GetStringSafe(index++),
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
                            Standing = reader.GetStringSafe(index++),
                            AddressID = reader.GetInt32Safe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }
                    
                    return returnCollection;
                }
            }
        }

        public int AddNewClient(object[] array)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "INSERT INTO tblClients (NewPEID, ClientName, OfficeName, Standing) " +
                        "VALUES (?,?,?,?)";
                    sqlCommand.Parameters.AddWithValue("NewPEID", array[0]);
                    sqlCommand.Parameters.AddWithValue("ClientName", array[1]);
                    sqlCommand.Parameters.AddWithValue("OfficeName", array[2] ?? DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("Standing", array[3]);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "Select @@identity";
                    return (int)sqlCommand.ExecuteScalar();
                }
            }
        }

        public void UpdateClientInformation(object array)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmDistinct(string clientName, string officeName)
        {
            throw new NotImplementedException();
        }
    }
}
