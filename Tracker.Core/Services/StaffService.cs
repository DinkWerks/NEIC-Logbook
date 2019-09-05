using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Tracker.Core.Extensions;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public class StaffService : BindableBase, IStaffService
    {
        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { SetProperty(ref _connectionString, value); }
        }

        public List<Staff> CompleteStaffList { get; set; }

        public StaffService()
        {
            SetConnectionString();
            CompleteStaffList = GetAllStaff();
        }

        public void SetConnectionString()
        {
            var dir = Settings.Settings.Instance.DatabaseAddress;
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir;
        }

        public List<Staff> GetAllStaff()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, PersonName, IsActive FROM tblStaff";
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    List<Staff> returnCollection = new List<Staff>();

                    while (reader.Read())
                    {
                        int index = 0;
                        Staff returnValue = new Staff()
                        {
                            ID = reader.GetInt32Safe(index++),
                            Name = reader.GetStringSafe(index++),
                            IsActive = reader.GetBooleanSafe(index++),
                            IsChanged = false
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }

        public void DeleteStaffMember(int id)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "DELETE FROM tblStaff WHERE ID = " + id;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            CompleteStaffList = GetAllStaff();
        }

        public int AddStaffMember(Staff newMember)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "INSERT INTO tblStaff (PersonName, IsActive) " +
                        "VALUES (?,?)";
                    sqlCommand.Parameters.AddWithValue("PersonName", newMember.Name ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("IsActive", newMember.IsActive);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "SELECT @@identity";
                    int newID = (int)sqlCommand.ExecuteScalar();

                    CompleteStaffList = GetAllStaff();
                    return newID;
                }
            }
        }

        public void UpdateStaffMember(Staff staff)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "UPDATE tblStaff SET " +
                        "PersonName=@pname, IsActive=@isactive WHERE ID = @id";
                    sqlCommand.Parameters.AddWithValue("@pname", staff.Name ?? Convert.DBNull);
                    sqlCommand.Parameters.AddWithValue("@isactive", staff.IsActive);
                    sqlCommand.Parameters.AddWithValue("@id", staff.ID);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            CompleteStaffList = GetAllStaff();
        }
    }
}
