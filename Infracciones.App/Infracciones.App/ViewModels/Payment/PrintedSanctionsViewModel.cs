using Infracciones.App.Models;
using Infracciones.App.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Infracciones.ViewModels.Payment
{
    public class PrintedSanctionsViewModel : BaseViewModel
    {
        private ObservableCollection<SanctionModel> _sanctions;

        private int _totalSanctions;

        public int TotalSanctions
        {
            get { return _totalSanctions; }
            set { _totalSanctions = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<SanctionModel> Sanctions
        {
            get { return _sanctions; }
            set { _sanctions = value; NotifyPropertyChanged(); }
        }

        public PrintedSanctionsViewModel(List<SanctionModel> sanctions)
        {
            Sanctions = new ObservableCollection<SanctionModel>();

            foreach (var sanction in sanctions)
            {
                sanction.SanctionReason.IconPath = string.Concat(App.App.BaseImageUrl, sanction.SanctionReason.IconPath.Replace("~/Media/", ""));
                Sanctions.Add(sanction);            
            }

            TotalSanctions = sanctions.Count;
        }
    }
}
