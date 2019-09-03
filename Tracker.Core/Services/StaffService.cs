using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public List<Staff> GetAllStaff()
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID, PersonName, IsActive" +
                        "FROM tblStaff";
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();
                    List<Staff> returnCollection = new List<Staff>();

                    while (reader.Read())
                    {
                        int index = 0;
                        Staff returnValue = new Staff()
                        {
                            ID = reader.GetInt32Safe(index++),
                            Name = reader.GetStringSafe(index++),
                            IsActive = reader.GetBooleanSafe(index++)
                        };
                        returnCollection.Add(returnValue);
                    }

                    return returnCollection;
                }
            }
        }
    }
}
