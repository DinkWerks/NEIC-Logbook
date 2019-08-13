using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using Tracker.Core.Extensions;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public class PersonService : IPersonService
    {
        private IAddressService _as;

        public Person CurrentPerson { get; set; }
        public List<Person> CompletePeopleList { get; set; }
        public string ConnectionString { get; set; }

        public PersonService(IAddressService addressService)
        {
            _as = addressService;

            SetConnectionString();
            CompletePeopleList = GetAllPartialPeople();
        }

        public void SetConnectionString()
        {
            var dir = Settings.Settings.Instance.DatabaseAddress;
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir;
        }

        public Person GetPersonByID(int id, bool loadAsCurrentClient = true)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, FirstName, LastName, CurrentAssociationID, CurrentAssociation, " +
                        "AddressID, Phone1, Phone2, Email, DisclosureLevel, Notes" +
                        " FROM tblPeople WHERE ID = " + id;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    if (!reader.HasRows)
                        return new Person();

                    reader.Read();

                    int index = 0;
                    Person returnValue = new Person()
                    {
                        ID = reader.GetInt32Safe(index++),
                        FirstName = reader.GetStringSafe(index++),
                        LastName = reader.GetStringSafe(index++),
                        CurrentAssociationID = reader.GetInt32Safe(index++),
                        CurrentAssociation = reader.GetStringSafe(index++),
                        AddressID = reader.GetInt32Safe(index),
                        AddressModel = _as.GetAddressByID(reader.GetInt32(index++)),
                        Phone1 = reader.GetStringSafe(index++),
                        Phone2 = reader.GetStringSafe(index++),
                        Email = reader.GetStringSafe(index++),
                        DisclosureLevel = reader.GetStringSafe(index++),
                        Note = reader.GetStringSafe(index++)
                    };

                    if (loadAsCurrentClient)
                    {
                        CurrentPerson = returnValue;
                    }
                    return returnValue;
                }
            }
        }

        public List<Person> GetPartialPeopleByCriteria(string criteria)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, FirstName, LastName, CurrentAssociation FROM tblPeople " + criteria;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    List<Person> returnCollection = new List<Person>();

                    while (reader.Read())
                    {
                        int index = 0;
                        Person returnValue = new Person()
                        {
                            ID = reader.GetInt32Safe(index++),
                            FirstName = reader.GetStringSafe(index++),
                            LastName = reader.GetStringSafe(index++),
                            CurrentAssociation = reader.GetStringSafe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public List<Person> GetAllPartialPeople()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, FirstName, LastName, CurrentAssociation FROM tblPeople";
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    List<Person> returnCollection = new List<Person>();

                    while (reader.Read())
                    {
                        int index = 0;
                        Person returnValue = new Person()
                        {
                            ID = reader.GetInt32Safe(index++),
                            FirstName = reader.GetStringSafe(index++),
                            LastName = reader.GetStringSafe(index++),
                            CurrentAssociation = reader.GetStringSafe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public int AddNewPerson(Person p)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "INSERT INTO tblPeople (FirstName, LastName) VALUEs (?,?)";
                    sqlCommand.Parameters.AddWithValue("FirstName", p.FirstName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("LastName", p.LastName ?? Convert.DBNull);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand.CommandText = "Select @@identity";
                    return (int)sqlCommand.ExecuteScalar();
                }
            }
        }

        public int UpdatePersonInformation(Person p)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID FROM tblPeople WHERE ID = ?";
                    sqlCommand.Parameters.AddWithValue("ID", p.ID);

                    connection.Open();
                    OleDbDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        using (OleDbConnection connection2 = new OleDbConnection(ConnectionString))
                        {
                            using (OleDbCommand updateCommand = connection2.CreateCommand())
                            {
                                updateCommand.CommandText = "UPDATE tblPeople SET FirstName = @fname, LastName = @lname, CurrentAssociationID = @assoc, CurrentAssociation = @assocname, " +
                                    "AddressID = @addr, Phone1 = @phone1, Phone2 = @phone2, Email = @email, DisclosureLevel = @disclosure, Notes = @notes " +
                                    "WHERE ID = @id";
                                updateCommand.Parameters.AddWithValue("@fname", p.FirstName ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@lname", p.LastName ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@assoc", p.CurrentAssociationID);
                                updateCommand.Parameters.AddWithValue("@assocname", p.CurrentAssociation ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@addr", p.AddressID);
                                updateCommand.Parameters.AddWithValue("@phone1", p.Phone1 ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@phone2", p.Phone2 ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@email", p.Email ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@disclosure", p.DisclosureLevel ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@notes", p.Note ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@id", p.ID);

                                connection2.Open();
                                updateCommand.ExecuteNonQuery();
                                connection2.Close();
                                return p.ID;
                            }
                        }
                    }
                    else
                    {
                        return AddNewPerson(p);
                    }
                }
            }
        }

        public void RemovePerson(int id, int addressID)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "DELETE FROM tblPeople WHERE ID = " + id;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();

                    _as.RemoveAddress(addressID);
                }
            }
        }

        public bool ConfirmDistinct(string firstName, string lastName)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID FROM tblPeople WHERE FirstName = @firstName AND LastName = @lastName";
                    sqlCommand.Parameters.AddWithValue("@firstname", firstName ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@lastName", lastName ?? Convert.DBNull);

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
