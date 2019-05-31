using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.Services
{
    public class DatabaseConnection : IDatabaseConnection
    {

        public SqlConnection Connection { get; set; }

        public void Connect()
        {
            string connectionString = @"C:\Users\kdeutsch\source\repos\Tracker\Tracker.Core\Resources\RS_Backend.accdb";
            Connection = new SqlConnection(connectionString);
        }

        public void Disconnect()
        {
            Connection.Close();
        }

        public SqlCommand GetData(string sqlCommand)
        {
            SqlCommand command = new SqlCommand(sqlCommand);
            return command;
        }

        public string GetState()
        {
            return Connection.State.ToString();
        }
    }
}
