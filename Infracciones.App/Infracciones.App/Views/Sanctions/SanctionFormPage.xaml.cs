using Infracciones.App.Models;
using Infracciones.App.Services;
using Infracciones.App.Services.DTO;
using Infracciones.App.Views.Home;
using Infracciones.App.Views.Identity;
using Infracciones.App.Views.Shared;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Sanctions
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SanctionFormPage : ContentPage
    {
        public SanctionFormPage()
        {
            InitializeComponent();
            EntPlates.Text = string.Empty;
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {                               
            try
            {
                if (string.IsNullOrEmpty(EntPlates.Text))
                    throw new Exception("Debe de ingresar el número de placa del vehículo.");

                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Buscando...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                var vehicle = new SanctionModel();
                vehicle.Plates = EntPlates.Text;

                var searchParameters = new SanctionSearchDTO
                {
                    TodaySanctions = false,
                    StartDate = null,
                    EndDate = null,
                    Plates = vehicle.Plates,
                    Officer = string.Empty,
                    Origin = 0,
                    SanctionReason = 0
                };

                var sanctions = await SanctionService.Search(searchParameters);
               
                if (sanctions.Count == 0)
                    throw new Exception("¡No se encontró ningún registro! Por favor, vuelva a intentarlo.");

                var sanction = sanctions.First();

                var details = new VehicleModel
                {
                    Plates = sanction.Plates,
                    Responsible = sanction.Responsible,
                    VehicleType = sanction.VehicleType,
                    Brand = sanction.Brand,
                    Subbrand = sanction.SubBrand,
                    Model = sanction.Model,
                    Origin = sanction.State,
                    CheckRepuveString = "No",
                    Sanctions = sanctions
                };
                
                var sanctionPage = new SanctionPage
                {
                    BindingContext = details
                };

                await Navigation.PopPopupAsync(true);
                await Navigation.PushAsync(sanctionPage, true);                
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
            ////await Navigation.PopAsync();
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }
    }
}