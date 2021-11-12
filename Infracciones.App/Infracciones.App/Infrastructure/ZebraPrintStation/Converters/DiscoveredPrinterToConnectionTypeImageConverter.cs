using Infracciones.Infrastructure.ZebraPrintStation.Services;
using System;
using System.Globalization;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.Converters
{
    public class DiscoveredPrinterToConnectionTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DiscoveredPrinter printer)
            {
                if (printer is DiscoveredPrinterNetwork)
                {
                    return "ic_wifi.png";
                }
                else if (DependencyService.Get<IPrinterHelper>().IsBluetoothPrinter(printer))
                {
                    return "ic_bluetooth.png";
                }
                else if (DependencyService.Get<IPrinterHelper>().IsUsbDirectPrinter(printer) || DependencyService.Get<IPrinterHelper>().IsUsbDriverPrinter(printer))
                {
                    return "ic_usb.png";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
