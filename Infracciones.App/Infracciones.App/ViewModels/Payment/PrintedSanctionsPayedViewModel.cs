using Infracciones.App.Models;
using Infracciones.App.ViewModels;
using System.Collections.Generic;

namespace Infracciones.ViewModels.Payment
{
    public class PrintedSanctionsPayedViewModel : BaseViewModel
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; NotifyPropertyChanged(); }
        }


        public PrintedSanctionsPayedViewModel(List<SanctionModel> sanctions)
        {
            for(var i = 0; i < sanctions.Count; i++)
            {
                Text += string.Format("\nComprobante de pago de: {0}\nReferencia: {1}\n", sanctions[i].SanctionReason.Description, sanctions[i].Payment.PaymentReference);
            }
        }
    }
}
