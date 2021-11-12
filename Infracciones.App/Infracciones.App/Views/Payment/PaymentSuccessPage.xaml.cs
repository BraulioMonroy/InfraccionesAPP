using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Views.Home;
using Infracciones.Infrastructure.ZebraPrintStation.Models;
using Infracciones.ViewModels.Sanction;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Payment;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Payment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentSuccessPage : ContentPage
    {
        private readonly List<SanctionModel> sanctions = new List<SanctionModel>();

        public PaymentSuccessPage(List<PaymentModel> payments)
        {
            InitializeComponent();

            BuildSanctions(payments);
            GetSanctions(payments);

            var paymentSuccessPopupView = new PaymentSuccessPopupView();
            Navigation.PushPopupAsync(paymentSuccessPopupView);
        }

        private async void GetSanctions(List<PaymentModel> payments)
        {
            try
            {
                foreach (var payment in payments) 
                {
                    var sanction = await SanctionService.GetById(payment.Sanction.Id);
                    sanctions.Add(sanction);
                }
            }
            catch(Exception e)
            {
                CloseAllPopup();

                var errorPopupView = new ErrorPopupView()
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = e.Message,
                        Image = "Error.png"
                    }
                };

                await Navigation.PushPopupAsync(errorPopupView);
            }
        }

        private void BuildSanctions(List<PaymentModel> payments)
        {
            var number = 1;

            foreach (var payment in payments)
            {
                switch (payment.Sanction.SanctionReasonId)
                {
                    //Emisión de Gases
                    case 1:
                        FrameEmisionDeGases.IsVisible = true;
                        ChkEmisionGases.IsChecked = true;
                        FolioEmisionDeGases.Text = payment.Sanction.Key;
                        NumberEmisionDeGases.Text = number.ToString();
                        break;

                    //Falta de Documentos
                    case 2:
                        FrameFaltaDeDocumentos.IsVisible = true;
                        ChkFaltaDeDocumentos.IsChecked = true;
                        FolioFaltaDeDocumentos.Text = payment.Sanction.Key;
                        NumberFaltaDeDocumentos.Text = number.ToString();
                        break;

                    //Circular en horario no permitido
                    case 3:
                        FrameCircularEnHorario.IsVisible = true;
                        ChkCircularEnHorario.IsChecked = true;
                        FolioCircularEnHorario.Text = payment.Sanction.Key;
                        NumberCircularEnHorario.Text = number.ToString();
                        break;
                }
                number++;
            }
        }

        private async void ButtonPrint_Clicked(object sender, EventArgs e)
        {
            //TODO: Add "Cargando" message
 
            var sanctions = new ListSanctionViewModel(this.sanctions);

            if (sanctions.Sanctions.Count < 1)
                throw new Exception("No hay sanciones seleccionadas para imprimir.");

            var printPage = new PrintStationPage(sanctions, FormatType.Payment);
            await Navigation.PushAsync(printPage, true);
        }

        private async void BtnEndProcess_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }
    }
}