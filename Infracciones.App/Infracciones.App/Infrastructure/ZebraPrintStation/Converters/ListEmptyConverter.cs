using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Infracciones.Infrastructure.ZebraPrintStation.Converters
{
    public class ListEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList list)
            {
                return list.Count <= 0;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
