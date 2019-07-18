using System;
namespace Tracker.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime dateTime)
        {
            return DateTime.Now.Date.ToLongDateString().Replace(DateTime.Now.DayOfWeek.ToString() + ", ", "");
        }
    }
}
