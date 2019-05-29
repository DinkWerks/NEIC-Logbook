using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace mClientList.Converters
{
    class StandingToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = String.Empty;
            switch (value)
            {
                case "Good Standing":
                    result = "bullet_green.png";
                    break;
                case "Warning":
                    result = "bullet_orange.png";
                    break;
                case "On Hold":
                    result = "bullet_red.png";
                    break;
                default:
                    break;
                    
            }
            return "/Tracker.Core;component/Resources/Icons/" + result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "/Tracker.Core;component/Resources/Icons/bullet_green.png":
                    return "Good Standing";
                case "/Tracker.Core;component/Resources/Icons/bullet_orange.png":
                    return "On Hold";
                case "/Tracker.Core;component/Resources/Icons/bullet_red.png":
                    return "On Hold";
                default:
                    return "Standing Unknown";
            }
        }
    }
}
