using System;
using System.Globalization;
using System.Windows.Data;

namespace mRecordSearchList.Converters
{
    public class ClientNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == null)
                return values[0];
            else
                return values[0] + " - " + values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
