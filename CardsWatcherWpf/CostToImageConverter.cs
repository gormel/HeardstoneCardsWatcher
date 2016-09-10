using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CardsWatcherWpf
{
    [ValueConversion(typeof(int), typeof(ImageSource))]
    public class CostToImageConverter : IValueConverter
    {
        private ImageSource mError;

        public CostToImageConverter()
        {
            
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
                return mError;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
