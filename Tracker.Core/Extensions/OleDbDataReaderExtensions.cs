using System;
using System.Data.OleDb;

namespace Tracker.Core.Extensions
{
    public static class OleDbDataReaderExtensions
    {
        public static bool GetBooleanSafe(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetBoolean(index);
            }
            return false;
        }

        public static int GetInt32Safe(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetInt32(index);
            }
            return 0;
        }

        public static float GetFloatSafe(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetFloat(index);
            }
            return 0;
        }

        public static decimal GetDecimalSafe(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetDecimal(index);
            }
            return 0;
        }

        public static string GetStringSafe(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetString(index);
            }
            return null;
        }

        public static string GetStringSpecial(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetString(index);
            }
            return string.Empty;
        }

        public static DateTime? GetDateTimeSafe(this OleDbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetDateTime(index);
            }
            return null;
        }
    }
}
