using Infracciones.Models;
using Infracciones.Services.PanicButton;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using System.Diagnostics;
using System.Collections;

namespace Infracciones.Views.PanicButton
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PanicButtonPopupView : PopupPage
    {
        public PanicButtonPopupView()
        {
            InitializeComponent();
        }

        private async void ToggleButton_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void PanicButton_Clicked(object sender, System.EventArgs e)
        {
            PanicButtonRequestModel panicButtonRequest = new PanicButtonRequestModel();

            var loading = new LoadingPopupView
            {
                BindingContext = new PopupViewModel()
                {
                    Text = "Enviando alerta...",
                    TextStyle = "PopupContentLabelStyle"
                }
            };

            try
            {
                await Navigation.PushPopupAsync(loading);

                decimal latitud = 19.4346412m;
                decimal longitud = -99.1765622m;
                string motivoActivacion = "21";  //21
                string descripcion = null;


                // Este objeto se manda al focus
                var panicButtonRequestFocus = new PanicButtonFocus
                {
                    descripcion = descripcion,
                    latitud = latitud,
                    longitud = longitud,
                    motivoActivacion = motivoActivacion,
                    nombreMedios = new ArrayList()
                };


                // Este objeto se manda al CAD
               /* panicButtonRequest = new PanicButtonRequestModel
                {
                    OriginId = 2,
                    Description = "BOTON AUXILIO VIAL SEDEMA",
                    MotiveId = 416, 
                    AddressId = 0, 
                    Latitude = 19.4346412m,
                    Longitude = -99.1765622m,
                    MobileUuid = 5,
                    Uuid = 5,
                    Priority = "ALTA",
                    OriginIntegration = "REAL",
                    Informer = new PanicButtonInformer(), //"{\"guardaDirectorio\":\"\",\"idEvento\":\"\",\"nombre\":\"\",\"telefono\":\"\",\"tipoDenunciante\":\"\",\"uuid\":\"5\"}",
                    CreatedBy = Xamarin.Essentials.Preferences.Get("userId", string.Empty)
                };*/



                var panicButton = await PanicButtonService.Add(panicButtonRequestFocus);
                await Navigation.PopAsync();
                PanicButton.Source = "panicButtonWhiteClicked.png";
                CloseAllPopup();

                //await DisplayAlert("Alerta enviada", $"No. Folio: {panicButton.SerialKey}", "Cerrar");
                ShowSuccessPopup(string.Concat("Alerta enviada. ", $"No. Folio: {panicButton[1]}"));                
            } catch (Exception)
            {
                CloseAllPopup();
                ShowErrorPopup("Servicio no disponible. Por favor, vuelva a intentarlo.");
            }
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        private async void ShowErrorPopup(string message)
        {
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

        private async void ShowSuccessPopup(string message)
        {
            var successPopupView = new ErrorPopupView()
            {
                BindingContext = new PopupViewModel()
                {
                    Text = message,
                    Image = "Success.png"
                }
            };

            await Navigation.PushPopupAsync(successPopupView);
        }
    }
}