using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.ViewModels.Vehicle;
using Infracciones.App.Views.Home;
using Infracciones.App.Views.Identity;
using Infracciones.App.Views.Sanctions;
using Infracciones.App.Views.Shared;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Vehicle
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleFoundPage : ContentPage
    {
        public VehicleFoundPage()
        {
            InitializeComponent();
        }

        private async void BtnMakeSanction_Clicked(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(EntResponsible.Text))
                //    throw new Exception("Debe de especificar el nombre del Responsable.");

                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "cargando...",
                        TextStyle = "popupcontentlabelstyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                //var popupView = new SanctionsPopupView
                //{
                //    BindingContext = this.BindingContext
                //};

                //await Navigation.PopPopupAsync(true);
                //await PopupNavigation.Instance.PushAsync(popupView, true);

                var vm = (VehicleFoundViewModel)BindingContext;

                var sanctionPage = new SanctionPage
                {
                    BindingContext = vm.Vehicle
                };

                await Navigation.PushAsync(sanctionPage, true);
                CloseAllPopup();
            }
            catch(Exception ex)
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

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            var cancelPopupView = new ErrorPopupView
            {
                BindingContext = new PopupViewModel()
                {
                    Text = "¡Consulta finalizada!",
                    Image = "icnBasuraRed.png"
                }
            };

            await Navigation.PushPopupAsync(cancelPopupView);
            await Task.Delay(1000);
            CloseAllPopup();

            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
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
    }
}