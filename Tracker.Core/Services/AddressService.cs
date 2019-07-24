using System;
using System.Data.OleDb;
using System.IO;
using Tracker.Core.Extensions;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public class AddressService : IAddressService
    {
        public string ConnectionString { get; set; }

        public AddressService()
        {
            SetConnectionString();
        }

        public void SetConnectionString()
        {
            //TODO Have this pull from a setting.
            var dir = Directory.GetCurrentDirectory();
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir + @"\Resources\RS_Backend.accdb";
        }

        public Address GetAddressByID(int id)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, AddressName, AttentionTo, Line1, Line2, City, " +
                        "State, Zip, Notes " +
                        "FROM tblAddresses WHERE ID = ?";
                    sqlCommand.Parameters.AddWithValue("ID", id);
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    if (!reader.HasRows)
                        return new Address();

                    int index = 0;
                    Address returnValue = new Address()
                    {
                        AddressID = reader.GetInt32Safe(index++),
                        AddressName = reader.GetStringSafe(index++),
                        AttentionTo = reader.GetStringSafe(index++),
                        AddressLine1 = reader.GetStringSafe(index++),
                        AddressLine2 = reader.GetStringSafe(index++),
                        City = reader.GetStringSafe(index++),
                        State = reader.GetStringSafe(index++),
                        ZIP = reader.GetStringSafe(index++),
                        Notes = reader.GetStringSafe(index++)
                    };

                    return returnValue;
                }
            }
        }

        public int AddNewAddress(Address a)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "INSERT INTO tblAddresses (AddressName, AttentionTo, Line1, Line2, City, State, Zip, Notes) " +
                        "VALUES (@AddressName @AttentionTo, @Line1, @Line2, @City, @State, @Zip, @Notes)";
                    sqlCommand.Parameters.AddWithValue("@AddressName", a.AddressName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@AttentionTo", a.AttentionTo ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@Line1", a.AddressLine1 ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@Line2", a.AddressLine2 ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@City", a.City ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@State", a.State ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@Zip", a.ZIP ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@Notes", a.Notes ?? Convert.DBNull);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "Select @@identity";
                    int newID = (int)sqlCommand.ExecuteScalar();
                    return newID;
                }
            }
        }

        public int UpdateAddress(Address a)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID FROM tblAddresses WHERE ID = ?";
                    sqlCommand.Parameters.AddWithValue("ID", a.AddressID);
                    connection.Open();
                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        using (OleDbConnection connection2 = new OleDbConnection(ConnectionString))
                        {
                            using (OleDbCommand updateCommand = connection2.CreateCommand())
                            {
                                updateCommand.CommandText = "UPDATE tblAddresses SET AddressName = @addressname, AttentionTo = @attnTo, Line1 = @line1, Line2 = @line2, " +
                                    "City = @city, State = @state, Zip = @zip, Notes = @notes " +
                                    "WHERE ID = @id";
                                updateCommand.Parameters.AddWithValue("@addressname", a.AddressName ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@attnTo", a.AttentionTo ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@line1", a.AddressLine1 ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@line2", a.AddressLine2 ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@city", a.City ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@state", a.State ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@zip", a.ZIP ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@notes", a.Notes ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@id", a.AddressID);

                                connection2.Open();
                                updateCommand.ExecuteNonQuery();
                                return a.AddressID;
                            }
                        }
                    }
                    else
                        return AddNewAddress(a);
                }
            }
        }
    }
}
