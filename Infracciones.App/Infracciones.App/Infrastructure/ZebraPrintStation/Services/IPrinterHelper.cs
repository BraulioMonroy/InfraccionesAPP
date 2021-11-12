using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.Services
{
    public interface IPrinterHelper
    {
        bool IsBluetoothPrinter(DiscoveredPrinter printer);

        bool IsUsbDirectPrinter(DiscoveredPrinter printer);

        bool IsUsbDriverPrinter(DiscoveredPrinter printer);
    }
}
