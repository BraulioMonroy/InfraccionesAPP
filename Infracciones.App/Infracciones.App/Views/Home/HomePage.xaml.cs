using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Views.AirQuality;
using Infracciones.App.Views.CashClosing;
using Infracciones.App.Views.Identity;
using Infracciones.App.Views.Payment;
using Infracciones.App.Views.Repuve;
using Infracciones.App.Views.Sanctions;
using Infracciones.App.Views.Shared;
using Infracciones.App.Views.Vehicle;
using Infracciones.Helpers;
using Infracciones.Services.Sqlite.MediaService;
using Infracciones.ViewModels.CashClosing;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Payment;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Home
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private static SqlMediaService _sqlMediaService = new SqlMediaService();

        public HomePage()
        {
            InitializeComponent();
            ClearData();
        }

        private async void ProfileButton_Clicked(object sender, EventArgs e)
        {
            var profilePopupView = new ProfilePopupView();
            await Navigation.PushPopupAsync(profilePopupView);
        }

        private async void MainMenu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Cargando...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                var key = e.Item as HomeModel;
                switch (key.Sectionkey)
                {
                    case "Vehicle":
                        await Navigation.PushAsync(new SearchVehiclePage(), true);
                        CloseAllPopup();
                        break;
                    case "Sanction":
                        await Navigation.PushAsync(new SanctionFormPage(), true);
                        CloseAllPopup();
                        break;
                    case "Payment":
                        await Navigation.PushAsync(new PaymentFormPage(), true);
                        CloseAllPopup();
                        break;
                    case "CashClosing":
                        var userId = Xamarin.Essentials.Preferences.Get("userId", string.Empty);
                        if (string.IsNullOrWhiteSpace(userId)) await DisplayAlert("Imposible obtener información de la sesión", "No fue posible obtener la información de su sesión. Favor de volver a ingresar a su perfil nuevamente.", "Aceptar");
                        else
                        {
                            var payments = await PaymentService.GetPaymentOpenByStatusAndOfficerId(true, PaymentStatus.Pagado.ToString(), userId);
                            var vm = new CashClosingViewModel(payments);
                            await Navigation.PushAsync(new CashClosingPage() { BindingContext = vm }, true);
                        }
                        CloseAllPopup();
                        break;
                    case "Repuve":
                        await Navigation.PushAsync(new RepuveFormPage(), true);
                        CloseAllPopup();
                        break;
                    case "AirQuality":
                        await Navigation.PushAsync(new AirQualityPage(), true);
                        CloseAllPopup();
                        break;
                }
            }
            catch (Exception ex)
            {
                CloseAllPopup();

                var errorPopupView = new ErrorPopupView()
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = ex.Message,
                        Image = "Error.png"
                    }
                };

                await Navigation.PushPopupAsync(errorPopupView);
            }
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void ClearData()
        {
            // Remove previous images
            await _sqlMediaService.RemoveAll();
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

       
    }
}