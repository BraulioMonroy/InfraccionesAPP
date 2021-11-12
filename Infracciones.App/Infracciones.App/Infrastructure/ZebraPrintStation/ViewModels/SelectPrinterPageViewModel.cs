using Infracciones.App.ViewModels;
using System.Collections.ObjectModel;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.ViewModels
{
    public class SelectPrinterPageViewModel : BaseViewModel
    {
        private DiscoveredPrinter highlightedPrinter;

        private bool isPrinterListRefreshing = false;
        private bool isSelectingPrinter = false;

        public DiscoveredPrinter HighlightedPrinter
        {
            get => highlightedPrinter;
            set
            {
                highlightedPrinter = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsPrinterListRefreshing
        {
            get => isPrinterListRefreshing;
            set
            {
                isPrinterListRefreshing = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSelectingPrinter
        {
            get => isSelectingPrinter;
            set
            {
                isSelectingPrinter = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DiscoveredPrinter> DiscoveredPrinterList { get; } = new ObservableCollection<DiscoveredPrinter>();
    }
}
