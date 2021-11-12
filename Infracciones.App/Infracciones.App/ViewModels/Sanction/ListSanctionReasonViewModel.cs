using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.ViewModels;
using System.Collections.ObjectModel;

namespace Infracciones.ViewModels.Sanction
{
    public class ListSanctionReasonViewModel : BaseViewModel
    {
        private ObservableCollection<SanctionReasonModel> _sanctionReasons;

        public ObservableCollection<SanctionReasonModel> SanctionReasons
        {
            get { return _sanctionReasons; }
            set { _sanctionReasons = value; NotifyPropertyChanged(); }
        }

        public ListSanctionReasonViewModel()
        {
            SanctionReasons = new ObservableCollection<SanctionReasonModel>();
            GetSanctionReasonCollection();
        }

        private async void GetSanctionReasonCollection()
        {
            var list = await SanctionReasonService.GetAll();

            foreach (var item in list)
                SanctionReasons.Add(item);
        }
    }
}
