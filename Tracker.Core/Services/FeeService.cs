using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
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

        public Fee GetFeeData(Fee returnValue, bool loadAsCurrentSearch = true)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                using (OleDbCommand sqlCommand = connection.CreateCommand())
                {
                    string fields = "";
                    List<FieldTypes> fieldTypes = new List<FieldTypes>();

                    //Parse each charge in returnValue, pull count/isChecked from DB, update costs if needed.
                    foreach (ICharge charge in returnValue.Charges)
                    {
                        if (charge.Type == "variable" || charge.Type == "categorical")
                        {
                            fields += charge.DBField + ", ";
                            fieldTypes.Add(FieldTypes.numerical);
                        }
                        else if (charge.Type == "boolean")
                        {
                            fields += charge.DBField + ", ";
                            fieldTypes.Add(FieldTypes.boolean);
                        }
                    }

                    sqlCommand.CommandText = "SELECT " + fields.Substring(0, fields.Length - 2) + " FROM tblFees WHERE ID = " + returnValue.ID;
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

                    returnValue.CalculateProjectCost();
                    return returnValue;
                }
            }
        }
    }
}
