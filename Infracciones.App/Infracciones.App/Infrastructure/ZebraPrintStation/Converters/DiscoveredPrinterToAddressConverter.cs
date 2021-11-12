using System;
using System.Globalization;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.Converters
{
    public class DiscoveredPrinterToAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DiscoveredPrinter printer ? printer.Address : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
