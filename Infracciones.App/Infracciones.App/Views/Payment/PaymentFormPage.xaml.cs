using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Views.Payment;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Payment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentFormPage : ContentPage
    {
        public PaymentFormPage()
        {
            InitializeComponent();
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(EntReference.Text))
                    throw new Exception("Debe de ingresar el número de Referencia de Pago.");

                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Buscando...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);
                
                var payment = await PaymentService.GetByReference(EntReference.Text.Trim());

                if (payment == null)
                    throw new Exception("¡No se encontró ningún registro! Por favor, vuelva a intentarlo.");

                if(string.Equals(payment.Status, "Pagado"))
                    throw new Exception(string.Format("Esta referencia ya fue pagada."));

                var sanctions = new List<SanctionModel>();
                var sanction = new SanctionModel();
                sanction = payment.Sanction; // set sanction
                sanction.Payment = payment; // set payment
                
                sanctions.Add(sanction);

                var paymentReference = new PaymentReferenceFoundPage(sanctions);
                await Navigation.PopPopupAsync(true);
                await Navigation.PushAsync(paymentReference, true);
            }
            catch (Exception ex)
            {
                CloseAllPopup();

                var errorPopupView = new ErrorPopupView()
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = ex.Message,
                        Image = "IcnPlacaRed.png"
                    }
                };

                await Navigation.PushPopupAsync(errorPopupView);
            }
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }
    }
}