using Xamarin.Forms;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.Services
{
    public static class ConnectionCreator
    {
        public static Connection Create(DiscoveredPrinter printer)
        {
            if (DependencyService.Get<IPrinterHelper>().IsUsbDirectPrinter(printer))
            {
                return DependencyService.Get<IConnectionManager>().GetUsbDirectConnection(printer.Address);
            }
            else if (DependencyService.Get<IPrinterHelper>().IsUsbDriverPrinter(printer))
            {
                return DependencyService.Get<IConnectionManager>().GetUsbDriverConnection(printer.Address);
            }
            else if (DependencyService.Get<IPrinterHelper>().IsBluetoothPrinter(printer))
            {
                return DependencyService.Get<IConnectionManager>().GetBluetoothConnection(printer.Address);
            }
            else
            {
                return new TcpConnection(printer.Address, 9100);
            }
        }
    }
}
