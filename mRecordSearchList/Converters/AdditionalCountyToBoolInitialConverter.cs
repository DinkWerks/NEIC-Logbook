using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.Converters
{
    public class AdditionalCountyToBoolInitialConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] is County)
            {
                ObservableCollection<County> selectedCounties = values[0] as ObservableCollection<County>;
                County itemCounty = values[1] as County;
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
