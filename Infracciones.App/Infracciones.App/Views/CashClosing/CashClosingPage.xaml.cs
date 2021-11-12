using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Views.Home;
using Infracciones.ViewModels.CashClosing;
using Infracciones.Views.PanicButton;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.CashClosing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashClosingPage : ContentPage
    {
        public CashClosingPage()
        {
            InitializeComponent();
        }

        private async void BtnClosing_Clicked(object sender, EventArgs e)
        {
            var vm = (CashClosingViewModel)BindingContext;
            ErrorLabel.Text = string.Empty;

            if (vm.TotalSanctions == 0)
            {
                ErrorLabel.Text = "No hay registros para enviar.";
                return;
            }

            try
            {
                var sendingPopupView = new CashClosingSendingPopupView();
                await Navigation.PushPopupAsync(sendingPopupView);

                var userId = Xamarin.Essentials.Preferences.Get("userId", string.Empty);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    CloseAllPopup();
                    throw new Exception("No fue posible obtener la información de su sesión. Favor de volver a ingresar a su perfil nuevamente.");
                }

                var shiftId = Xamarin.Essentials.Preferences.Get("ShiftId", 0);
                if (shiftId <= 0)
                {
                    CloseAllPopup();
                    throw new Exception("No fue posible obtener la información de su sesión. Favor de volver a ingresar a su perfil nuevamente.");
                }

                CashClosingDtoModel cashClosingEntity = new CashClosingDtoModel
                {
                    OfficerId = userId,
                    CreatedBy = userId,
                    ShiftId = shiftId,
                    Payments = vm.SanctionsPaid.Select(x => new PaymentModel { SanctionId = x.SanctionId, ClosedBy = userId })
                };

                var cashClosing = await CashClosingService.Add(cashClosingEntity);

                vm.RefreshingCommand.Execute(null);

                //var emptyList = new List<SanctionsPaid>();
                //DataGrid.ItemsSource = emptyList;
                //vm.IsEmpty = true;

                await Navigation.RemovePopupPageAsync(sendingPopupView);

                var successPopupView = new CashClosingSentSuccessPopupView();
                await Navigation.PushPopupAsync(successPopupView);

                await Task.Delay(1000);
                CloseAllPopup();
            }
            catch (Exception ex)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await DisplayAlert("Error", "No fue posible enviar el corte de caja. " + ex.Message, "Continuar");
                vm.RefreshingCommand.Execute(null);
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
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