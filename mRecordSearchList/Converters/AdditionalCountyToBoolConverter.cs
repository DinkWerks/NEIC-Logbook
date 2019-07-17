using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.Converters
{
    public class AdditionalCountyToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((values[0] is bool) && (bool)values[0] == true)
            {
                return true;
            }
            else if (values[2] is County)
            {
                ObservableCollection<County> selectedCounties = values[1] as ObservableCollection<County>;
                County itemCounty = values[2] as County;
                if (selectedCounties.Contains(itemCounty))
                    return true;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BooleanAndConverter is a OneWay converter.");
        }
    }
}
