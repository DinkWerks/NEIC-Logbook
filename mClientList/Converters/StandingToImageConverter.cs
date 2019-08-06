using System;
using System.Globalization;
using System.Windows.Data;

namespace mClientList.Converters
{
    public class StandingToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "/Tracker.Core;component/Resources/Icons/" + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
