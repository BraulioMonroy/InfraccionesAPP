using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Infracciones.ViewModels.CashClosing
{
    public class CashClosingViewModel : BaseViewModel
    {
        #region Fields
        #endregion

        #region Property

        public bool IsEmpty { get; set; }
        public bool IsRefreshing { get; set; }
        public int TotalSanctions { get; set; }
        public ObservableCollection<SanctionsPaid> SanctionsPaid { get; set; }
        public Command RefreshingCommand { get; set; }

        #endregion

        #region Constructor

        public CashClosingViewModel(List<PaymentModel> payments)
        {
            var sanctions = BindSanctions(payments);
            SanctionsPaid = new ObservableCollection<SanctionsPaid>(sanctions);
            TotalSanctions = SanctionsPaid.Count;
            IsEmpty = !(TotalSanctions > 0);
            IsRefreshing = false;

            RefreshingCommand = new Command(async () => await RefreshSanctions());
        }

        private List<SanctionsPaid> BindSanctions(List<PaymentModel> payments)
        {
            List<SanctionsPaid> sanctions = new List<SanctionsPaid>();

            foreach (var item in payments)
            {
                if (!item.Closed)
                {
                    var sanctionPaid = new SanctionsPaid
                    {
                        SanctionId = item.SanctionId,
                        Number = item.Sanction.Key,
                        Date = item.PaymentDate.Value,
                        Plates = item.Sanction.Plates,
                        Officer = "David Alvarado",
                        Amount = item.Amount.ToString("##.00")
                    };
                    sanctions.Add(sanctionPaid);
                }
            }

            return sanctions;
        }

        private async Task RefreshSanctions()
        {
            IsRefreshing = true;

            var payments = await PaymentService.GetSanctionsPaid();
            var sanctions = BindSanctions(payments);
            SanctionsPaid = new ObservableCollection<SanctionsPaid>(sanctions);
            TotalSanctions = SanctionsPaid.Count;
            IsEmpty = !(TotalSanctions > 0);

            IsRefreshing = false;
        }

        #endregion
    }
}
