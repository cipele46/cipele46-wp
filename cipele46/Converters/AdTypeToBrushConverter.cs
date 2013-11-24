using cipele46.Model;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace cipele46.Converters
{
    public class AdTypeToBrushConverter : IValueConverter
    {
        public static SolidColorBrush SupplyBrush = new SolidColorBrush(Color.FromArgb(255, 23, 193, 199));
        public static SolidColorBrush DemandBrush = new SolidColorBrush(Color.FromArgb(255, 244, 113, 91));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((types)value == types.supply)
                return SupplyBrush;
            else
                return DemandBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
