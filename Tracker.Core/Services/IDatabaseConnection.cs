using System.Data;
using System.Data.SqlClient;

namespace Tracker.Core.Services
{
    public interface IDatabaseConnection
    {
        SqlConnection Connection { get; set; }
        void Connect();
        void Disconnect();
        string GetState();
        SqlCommand GetData(string sqlCommand);
    }
}
