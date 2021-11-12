using Infracciones.App.ViewModels;
using Infracciones.Infrastructure.ZebraPrintStation.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Infracciones.Infrastructure.ZebraPrintStation.ViewModels
{
    public class PrintFormatPageViewModel : BaseViewModel
    {
        private bool isVariableFieldListRefreshing = false;
        private bool isSendingPrintJob = false;

        public bool IsVariableFieldListRefreshing
        {
            get => isVariableFieldListRefreshing;
            set
            {
                isVariableFieldListRefreshing = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsSendingPrintJob
        {
            get => isSendingPrintJob;
            set
            {
                isSendingPrintJob = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<FormatVariable> FormatVariableList { get; } = new ObservableCollection<FormatVariable>();

        public PrintFormatPageViewModel()
        {
            FormatVariableList.CollectionChanged += FormatVariableList_CollectionChanged;
        }

        private void FormatVariableList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(FormatVariableList));
        }
    }
}
