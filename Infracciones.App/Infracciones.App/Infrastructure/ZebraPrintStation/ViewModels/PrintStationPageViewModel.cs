using Infracciones.App.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Zebra.Sdk.Printer.Discovery;

namespace Infracciones.Infrastructure.ZebraPrintStation.ViewModels
{
    public class PrintStationPageViewModel : BaseViewModel
    {
        private DiscoveredPrinter selectedPrinter;
        private bool isStoredFormatListRefreshing = false;
        private bool isPrinterFormatListRefreshing = false;
        private bool isSelectingNfcPrinter = false;
        private bool isSavingFormat = false;
        private bool isDeletingFormat = false;

        public DiscoveredPrinter SelectedPrinter
        {
            get => selectedPrinter;
            set
            {
                selectedPrinter = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsStoredFormatListRefreshing
        {
            get => isStoredFormatListRefreshing;
            set
            {
                isStoredFormatListRefreshing = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsPrinterFormatListRefreshing
        {
            get => isPrinterFormatListRefreshing;
            set
            {
                isPrinterFormatListRefreshing = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSelectingNfcPrinter
        {
            get => isSelectingNfcPrinter;
            set
            {
                isSelectingNfcPrinter = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSavingFormat
        {
            get => isSavingFormat;
            set
            {
                isSavingFormat = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsDeletingFormat
        {
            get => isDeletingFormat;
            set
            {
                isDeletingFormat = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<FormatViewModel> StoredFormatList { get; } = new ObservableCollection<FormatViewModel>();

        public ObservableCollection<FormatViewModel> PrinterFormatList { get; } = new ObservableCollection<FormatViewModel>();

        public PrintStationPageViewModel()
        {
            StoredFormatList.CollectionChanged += StoredFormatList_CollectionChanged;
            PrinterFormatList.CollectionChanged += PrinterFormatList_CollectionChanged;
        }

        private void StoredFormatList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(StoredFormatList));
        }

        private void PrinterFormatList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(PrinterFormatList));
        }
    }
}
