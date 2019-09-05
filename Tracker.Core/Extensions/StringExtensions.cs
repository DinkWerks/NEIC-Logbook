using System.Windows.Media;

namespace Tracker.Core.Extensions
{
    public static class StringExtensions
    {
        public static SolidColorBrush ToBrush(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return (SolidColorBrush) new BrushConverter().ConvertFromString(value);
            }
            return null;
        }

        public static Color ToColor(this string hex)
        {
            return (Color)ColorConverter.ConvertFromString(hex);
        }
    }
}
