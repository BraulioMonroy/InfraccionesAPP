using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Views.Home;
using Infracciones.Infrastructure.ZebraPrintStation.Models;
using Infracciones.ViewModels.Payment;
using Infracciones.ViewModels.Sanction;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Payment
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentReferenceFoundPage : ContentPage
    {
        private decimal _total;
        private readonly List<SanctionModel> _sanctions;

        public PaymentReferenceFoundPage(List<SanctionModel> sanctions)
        {
            InitializeComponent();
            _total = 0;
            _sanctions = sanctions;
            BuildSanctions(sanctions);
        }

        private void BuildSanctions(List<SanctionModel> sanctions)
        {
            var number = 1;
            foreach(var sanction in sanctions)
            {
                
                switch (sanction.SanctionReasonId)
                {
                    //Emisión de Gases
                    case 1: FrameEmisionDeGases.IsVisible = true;
                        ChkEmisionGases.IsChecked = true;
                        FolioEmisionDeGases.Text = sanction.Key;
                        MontoEmisionDeGases.Text = string.Format("{0:C2}", sanction.Amount);
                        NumberEmisionDeGases.Text = number.ToString();
                        _total += sanction.Amount;
                        break;

                    //Falta de Documentos
                    case 2:
                        FrameFaltaDeDocumentos.IsVisible = true;
                        ChkFaltaDeDocumentos.IsChecked = true;
                        FolioFaltaDeDocumentos.Text = sanction.Key;
                        MontoFaltaDeDocumentos.Text = string.Format("{0:C2}", sanction.Amount);
                        NumberFaltaDeDocumentos.Text = number.ToString();
                        _total += sanction.Amount;
                        break;

                    //Circular en horario no permitido
                    case 3:
                        FrameCircularEnHorario.IsVisible = true;
                        ChkCircularEnHorario.IsChecked = true;
                        FolioCircularEnHorario.Text = sanction.Key;
                        MontoCircularEnHorario.Text = string.Format("{0:C2}", sanction.Amount);
                        NumberCircularEnHorario.Text = number.ToString();
                        _total += sanction.Amount;
                        break;
                }
                number++;
            }

            Total.Text = string.Format("{0:C2}", _total);
        }

        private void ChkEmisionGases_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            decimal montoEmision = !string.IsNullOrEmpty(MontoEmisionDeGases.Text) ? Convert.ToDecimal(MontoEmisionDeGases.Text.Replace("$", "").Replace(",", "")) : 0; ;

            if (!(sender as CheckBox).IsChecked)
            {
                ChkEmisionGases.Color = Color.FromHex("#C9C9C9");
                FrameIcnEmisionDeGases.BackgroundColor = Color.FromHex("#C9C9C9");

                _total -= montoEmision;
                Total.Text = string.Format("{0:C2}", _total);
            }
            else
            {
                ChkEmisionGases.Color = Color.FromHex("#00B140");
                FrameIcnEmisionDeGases.BackgroundColor = Color.FromHex("#00B140");
                _total += montoEmision;
                Total.Text = string.Format("{0:C2}", _total);
            }
        }

        private void ChkFaltaDeDocumentos_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            decimal montoDocumentos = !string.IsNullOrEmpty(MontoFaltaDeDocumentos.Text) ? Convert.ToDecimal(MontoFaltaDeDocumentos.Text.Replace("$", "").Replace(",", "")) : 0; ;

            if (!(sender as CheckBox).IsChecked)
            {
                ChkFaltaDeDocumentos.Color = Color.FromHex("#C9C9C9");
                FrameIcnFaltaDeDocumentos.BackgroundColor = Color.FromHex("#C9C9C9");
                _total -= montoDocumentos;
                Total.Text = string.Format("{0:C2}", _total);
            }
            else
            {
                ChkFaltaDeDocumentos.Color = Color.FromHex("#00B140");
                FrameIcnFaltaDeDocumentos.BackgroundColor = Color.FromHex("#00B140");
                _total += montoDocumentos;
                Total.Text = string.Format("{0:C2}", _total);
            }
        }

        private void ChkCircularEnHorario_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            decimal montoCircular = !string.IsNullOrEmpty(MontoCircularEnHorario.Text) ? Convert.ToDecimal(MontoCircularEnHorario.Text.Replace("$", "").Replace(",", "")) : 0; ;

            if (!(sender as CheckBox).IsChecked)
            {
                ChkCircularEnHorario.Color = Color.FromHex("#C9C9C9");
                FrameIcnCircularEnHorario.BackgroundColor = Color.FromHex("#C9C9C9");
                _total -= montoCircular;
                Total.Text = string.Format("{0:C2}", _total);
            }
            else
            {
                ChkCircularEnHorario.Color = Color.FromHex("#00B140");
                FrameIcnCircularEnHorario.BackgroundColor = Color.FromHex("#00B140");
                _total += montoCircular;
                Total.Text = string.Format("{0:C2}", _total);
            }
        }

        private async void BtnPay_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Loading("Procesando...");

                var sanctionsToPay = new PrintedSanctionsViewModel(GetSelectedSanctions());

                if (sanctionsToPay.Sanctions.Count < 1)
                    throw new Exception("No hay sanciones seleccionadas para pagar.");

                var paymentList = new List<PaymentModel>();

                foreach (var sanction in sanctionsToPay.Sanctions)
                {
                    var payment = new PaymentModel
                    {
                        Id = sanction.Payment.Id,
                        PaymentReference = sanction.Payment.PaymentReference,
                        Status = "Pagado",
                        Card = "1234-5678-9012-3456"
                    };

                    var transaction = await PaymentService.Pay(payment.PaymentReference, payment);
                    paymentList.Add(transaction);
                }

                CloseAllPopup();

                var successPage = new PaymentSuccessPage(paymentList);
                await Navigation.PushAsync(successPage, true);
            }
            catch (Exception ex)
            {
                await DisplayError(ex.Message);
            }
        }

        private async void ButtonPrint_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Loading("Procesando...");

                var sanctions = new ListSanctionViewModel(GetSelectedSanctions());
                
                if (sanctions.Sanctions.Count < 1)
                    throw new Exception("No hay sanciones seleccionadas para imprimir.");

                CloseAllPopup();

                var printStation = new PrintStationPage(sanctions, FormatType.Sanction);
                await Navigation.PushAsync(printStation, true);
            }
            catch(Exception ex)
            {
                await DisplayError(ex.Message);
            }
        }

        private async Task DisplayError(string message)
        {
            CloseAllPopup();

            var errorPopupView = new ErrorPopupView()
            {
                BindingContext = new PopupViewModel()
                {
                    Text = message,
                    Image = "Error.png"
                }
            };

            await Navigation.PushPopupAsync(errorPopupView);
        }

        private async Task Loading(string message)
        {
            var loading = new LoadingPopupView
            {
                BindingContext = new PopupViewModel()
                {
                    Text = message,
                    TextStyle = "PopupContentLabelStyle"
                }
            };

            await Navigation.PushPopupAsync(loading, true);
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }   

        private List<SanctionModel> GetSelectedSanctions()
        {
            var selected = new List<SanctionModel>();

            if (ChkEmisionGases.IsChecked)
                selected.Add(_sanctions.Where(s => s.SanctionReasonId == 1).FirstOrDefault());

            if (ChkFaltaDeDocumentos.IsChecked)
                selected.Add(_sanctions.Where(s => s.SanctionReasonId == 2).FirstOrDefault());

            if (ChkCircularEnHorario.IsChecked)
                selected.Add(_sanctions.Where(s => s.SanctionReasonId == 3).FirstOrDefault());

            return selected;
        }
    }
}