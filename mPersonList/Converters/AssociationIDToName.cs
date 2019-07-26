using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Tracker.Core.Services;
using Unity;
using Unity.Extension;

namespace mPersonList.Converters
{
    public class AssociationIDToName : IValueConverter
    {
        private IClientService _cs;

        public AssociationIDToName()
        {
            UnityContainerExtension unityContainer = (UnityContainerExtension)Application.Current.Resources["IoC"];
            _cs = unityContainer.Resolve<IClientService>();
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
