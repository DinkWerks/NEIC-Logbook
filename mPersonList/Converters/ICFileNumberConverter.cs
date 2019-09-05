using System;
using System.Globalization;
using System.Windows.Data;

namespace mPersonList.Converters
{
    public class ICFileNumberConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace((string)values[3]))
                return string.Format("{0}{1}-{2}", values);
            else
                return string.Format("{0}{1}-{2}{3}", values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
