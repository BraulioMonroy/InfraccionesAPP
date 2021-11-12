using Infracciones.Infrastructure.ZebraPrintStation.Services;
using System;
using System.Globalization;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.Converters
{
    public class DiscoveredPrinterToFriendlyNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DiscoveredPrinter printer)
            {
                if (printer is DiscoveredPrinterNetwork && printer.DiscoveryDataMap.ContainsKey("SYSTEM_NAME"))
                {
                    return printer.DiscoveryDataMap["SYSTEM_NAME"];
                }
                else if (DependencyService.Get<IConnectionManager>().IsBluetoothPrinter(printer) && printer.DiscoveryDataMap.ContainsKey("FRIENDLY_NAME"))
                {
                    return printer.DiscoveryDataMap["FRIENDLY_NAME"];
                }
                else if (printer.DiscoveryDataMap.ContainsKey("SERIAL_NUMBER"))
                {
                    return printer.DiscoveryDataMap["SERIAL_NUMBER"];
                }
                else
                {
                    return printer.Address;
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
