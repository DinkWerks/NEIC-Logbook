using System;
using System.Globalization;
using System.Windows.Data;

namespace mRecordSearchList.Converters
{
    public class RSIDFormatter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace((string)values[3]))
                return String.Format("{0}-{1}-{2}", values);
            else
                return String.Format("{0}-{1}-{2}-{3}", values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
