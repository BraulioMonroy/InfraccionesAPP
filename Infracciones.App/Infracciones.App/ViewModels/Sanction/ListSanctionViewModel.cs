using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Infracciones.ViewModels.Sanction
{
    public class ListSanctionViewModel : BaseViewModel
    {
        private ObservableCollection<SanctionModel> _sanctions;

        public ObservableCollection<SanctionModel> Sanctions
        {
            get { return _sanctions; }
            set { _sanctions = value; NotifyPropertyChanged(); }
        }

        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set { _total = value; NotifyPropertyChanged(); }
        }

        //private SanctionModel _selectedSanction;

        //public SanctionModel SelectedSanction
        //{
        //    get { return _selectedSanction; }
        //    set { _selectedSanction = value; NotifyPropertyChanged(); }
        //}

        public ListSanctionViewModel(List<SanctionModel> sanctions)
        {
            Sanctions = new ObservableCollection<SanctionModel>();

            foreach (var sanction in sanctions)
            {                
                sanction.SanctionReason.IconPath = string.Concat(App.App.BaseImageUrl, sanction.SanctionReason.IconPath.Replace("~/Media/", ""));
                Sanctions.Add(sanction);
                Total += sanction.Amount;
            }
        }       
    }
}
    