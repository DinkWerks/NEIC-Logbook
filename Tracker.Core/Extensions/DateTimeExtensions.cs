using System;
namespace Tracker.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime != null)
                return dateTime.Value.ToLongDateString().Replace(DateTime.Now.DayOfWeek.ToString() + ", ", "");
            else
                return string.Empty;
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return DateTime.Now.ToLongDateString().Replace(DateTime.Now.DayOfWeek.ToString() + ", ", "");
        }
    }
}
