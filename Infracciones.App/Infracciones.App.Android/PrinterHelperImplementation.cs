using Infracciones.Infrastructure.ZebraPrintStation.Services;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

[assembly: Dependency(typeof(Infracciones.App.Droid.PrinterHelperImplementation))]
namespace Infracciones.App.Droid
{
    public class PrinterHelperImplementation : IPrinterHelper
    {
        public bool IsBluetoothPrinter(DiscoveredPrinter printer)
        {
            return printer is DiscoveredPrinterBluetooth;
        }

        public bool IsUsbDirectPrinter(DiscoveredPrinter printer)
        {
            return printer is DiscoveredPrinterUsb;
        }

        public bool IsUsbDriverPrinter(DiscoveredPrinter printer)
        {
            return false; // No implementation for USB driver printers in Android portion of Xamarin SDK
        }
    }
}