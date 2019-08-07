using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Tracker.Core.Extensions;
using Tracker.Core.Models.Fees;

namespace Tracker.Core.Services
{
    public class FeeService : IFeeService
    {
        private enum FieldTypes { numerical, boolean }

        public string ConnectionString { get; set; }

        public FeeService()
        {
            SetConnectionString();
        }

        public void SetConnectionString()
        {
            //TODO Pull this from a setting
            var dir = Directory.GetCurrentDirectory();
            ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dir + @"\Resources\RS_Backend.accdb";
        }

        public Fee GetFeeData(Fee returnValue)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    string fields = returnValue.GetFieldNames();

                    sqlCommand.CommandText = "SELECT " + fields + ", Adjustment, AdjustmentExplanation FROM tblFees WHERE ID = " + returnValue.ID;
                    connection.Open();

                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    reader.Read();

                    int index = 0;
                    foreach (ICharge charge in returnValue.Charges)
                    {
                        switch (charge.Type)
                        {
                            case "variable":
                                VariableCharge vCharge = (VariableCharge)charge;
                                vCharge.Count = reader.GetDecimalSafe(index++);
                                break;
                            case "categorical":
                                CategoricalCharge cCharge = (CategoricalCharge)charge;
                                cCharge.Count = reader.GetDecimalSafe(index++);
                                break;
                            case "boolean":
                                BooleanCharge bCharge = (BooleanCharge)charge;
                                bCharge.IsIncurred = reader.GetBooleanSafe(index++);
                                break;
                        }
                    }
                    returnValue.Adjustment = reader.GetDecimalSafe(index++);
                    returnValue.AdjustmentExplanation = reader.GetStringSafe(index++);

                    returnValue.CalculateProjectCost();
                    return returnValue;
                }
            }
        }

        public int AddNewFee()
        {
            using (OleDbConnection feeConnection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand newFeeCommand = feeConnection.CreateCommand())
                {
                    newFeeCommand.CommandText = "INSERT INTO tblFees DEFAULT VALUES";

                    feeConnection.Open();
                    newFeeCommand.ExecuteNonQuery();
                    newFeeCommand.CommandText = "Select @@identity";
                    int newID = (int)newFeeCommand.ExecuteScalar();
                    return newID;
                }
            }
        }

        public int UpdateFee(Fee f)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    sqlCommand.CommandText = "SELECT ID FROM tblAddresses WHERE ID = ?";
                    sqlCommand.Parameters.AddWithValue("ID", f.ID);
                    connection.Open();
                    OleDbDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        using (OleDbConnection connection2 = new OleDbConnection(ConnectionString))
                        {
                            using (OleDbCommand updateCommand = connection2.CreateCommand())
                            {
                                updateCommand.CommandText = "UPDATE tblFees SET " + f.GetFieldNames("update") +
                                    ", Adjustment = @adj, AdjustmentExplanation = @adjexp " +
                                    "WHERE ID = @id";
                                foreach (ICharge charge in f.Charges)
                                {
                                    switch (charge.Type)
                                    {
                                        case "variable":
                                            VariableCharge vCharge = (VariableCharge)charge;
                                            updateCommand.Parameters.AddWithValue("@" + vCharge.DBField, vCharge.Count);
                                            break;
                                        case "categorical":
                                            CategoricalCharge cCharge = (CategoricalCharge)charge;
                                            updateCommand.Parameters.AddWithValue("@" + cCharge.DBField, cCharge.Count);
                                            break;
                                        case "boolean":
                                            BooleanCharge bCharge = (BooleanCharge)charge;
                                            updateCommand.Parameters.AddWithValue("@" + bCharge.DBField, bCharge.IsIncurred);
                                            break;
                                    }
                                }
                                updateCommand.Parameters.AddWithValue("@adj", f.Adjustment);
                                updateCommand.Parameters.AddWithValue("@adjexp", f.AdjustmentExplanation ?? Convert.DBNull);
                                updateCommand.Parameters.AddWithValue("@id", f.ID);

                                connection2.Open();
                                updateCommand.ExecuteNonQuery();
                                return f.ID;
                            }
                        }
                    }
                    else
                        return AddNewFee();
                }
            }
        }
    }
}
