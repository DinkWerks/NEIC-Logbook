using System;
using System.Globalization;
using System.Windows.Data;

namespace Tracker.Controls.Converters
{
    public class BoolToVisibilityInverted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
                return "Hidden";
            else
                return "Visible";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            if (stringValue == "Hidden")
                return true;
            else
                return false;
        }
    }
}
