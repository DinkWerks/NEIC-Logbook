﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace mFeeCalculator.Converters
{
    public class PresentToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (decimal)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
