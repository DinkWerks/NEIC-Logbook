using System;
using System.Globalization;
using System.Windows.Data;
using Tracker.Core.Services;
using Prism.Ioc;
using Prism.Unity;

namespace mPersonList.Converters
{
    public class AssociationIDToName : IValueConverter
    {
        private IClientService _cs;

        public AssociationIDToName()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int associationID = (int)value;
            return "text";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
