using Infracciones.App.Services;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Identity
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordRecoveryPage
    {
        public PasswordRecoveryPage()
        {
            InitializeComponent();
        }

        private async void BtnRecover_Clicked(object sender, EventArgs e)
        {
            try
            {               
                if (string.IsNullOrEmpty(EntEmail.Text))
                    throw new Exception("Debe de proporcionar la dirección de email.");

                if (!IdentityService.IsValidEmail(EntEmail.Text))
                    throw new Exception("La dirección de email no tiene un formato válido.");

                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Enviando...",
                    }
                };

                await Navigation.PushPopupAsync(loading);

                //Validate credentials through API
                var response = await IdentityService.RecoverPassword(EntEmail.Text);

                await Navigation.PopPopupAsync(true);

                if (!response)
                    throw new Exception("Correo institucional erróneo, favor de validar la información ingresada.");                

                var successPopupView = new SuccessPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Image = "Success.png",
                        Text = "¡Las instrucciones para recuperar su contraseña han sido enviadas a su dirección de email.",
                    }
                };

                await Navigation.PushPopupAsync(successPopupView);
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

        private async void ReturnToLoginTap_Tapped(object sender, EventArgs e)
        {
            // Set Login as MainPage and pop Password Recovery Page after back.
            Navigation.InsertPageBefore(new LoginPage(), this);
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