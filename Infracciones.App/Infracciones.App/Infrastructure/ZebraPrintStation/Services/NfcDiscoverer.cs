using System;
using System.Threading;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.Services
{
    public static class NfcDiscoverer
    {
        public static DiscoveredPrinter DiscoverPrinter(string nfcData)
        {
            DiscoveredPrinter printer = null;

            try
            {
                NfcDiscoveryHandler nfcDiscoveryHandler = new NfcDiscoveryHandler();
                DependencyService.Get<IConnectionManager>().FindUrlPrinters(nfcData, nfcDiscoveryHandler);

                while (!nfcDiscoveryHandler.IsFinished)
                {
                    Thread.Sleep(100);
                }

                printer = nfcDiscoveryHandler.PreferredPrinter;
            }
            catch (Exception) { }

            return printer;
        }

        private class NfcDiscoveryHandler : DiscoveryHandler
        {

            public bool IsFinished { get; private set; } = false;

            public DiscoveredPrinter PreferredPrinter { get; private set; } = null;

            public void DiscoveryError(string message)
            {
                IsFinished = true;
            }

            public void DiscoveryFinished()
            {
                IsFinished = true;
            }

            public void FoundPrinter(DiscoveredPrinter printer)
            {
                if (PreferredPrinter == null || 
                        (DependencyService.Get<IConnectionManager>().IsBluetoothPrinter(PreferredPrinter) && 
                            printer is DiscoveredPrinterNetwork))
                {
                    PreferredPrinter = printer;
                }
            }
        }
    }
}
