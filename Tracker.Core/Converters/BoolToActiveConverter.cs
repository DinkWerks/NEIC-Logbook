﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Tracker.Core.Converters
{
    public class BoolToActiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool active = (bool)value;

            if (active)
                return "Active";
            else
                return "Inactive";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string header = (string)value;

            if (header == "Active")
                return true;
            else
                return false;
        }
    }
}
