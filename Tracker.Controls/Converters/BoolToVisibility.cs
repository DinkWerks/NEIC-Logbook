using System;
using System.Globalization;
using System.Windows.Data;

namespace Tracker.Controls.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
                return "Visible";
            else
                return "Hidden";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            if (stringValue == "Visible")
                return true;
            else
                return false;
        }
    }
}
