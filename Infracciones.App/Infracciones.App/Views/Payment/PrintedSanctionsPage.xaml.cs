using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Views.Home;
using Infracciones.App.Views.Payment;
using Infracciones.ViewModels.Payment;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Payment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintedSanctionsPage : ContentPage
    {
        public PrintedSanctionsPage()
        {
            InitializeComponent();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void BtnEndProcess_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void BtnPay_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Loading("Procesando...");               

                var sanctionsToPay = (PrintedSanctionsViewModel)BindingContext;
                var paymentList = new List<PaymentModel>();

                foreach(var sanction in sanctionsToPay.Sanctions)
                {
                    //TODO: Retrieve card info from device or form
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
            catch(Exception ex)
            {
                await DisplayError(ex.Message);
            }
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
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
    }
}