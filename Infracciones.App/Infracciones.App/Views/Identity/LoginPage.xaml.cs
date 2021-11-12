using Infracciones.App.Services;
using Infracciones.App.Views.Home;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Identity
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void TapForgotPassword_Tapped(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new PasswordRecoveryPage());
            Navigation.InsertPageBefore(new PasswordRecoveryPage(), this);
            await Navigation.PopAsync();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EntEmail.Text) || string.IsNullOrEmpty(EntPassword.Text))
                    throw new Exception("Debe de ingresar un usuario y contraseña.");

                if (!IdentityService.IsValidEmail(EntEmail.Text))
                    throw new Exception("El formato de la dirección de email no es valido.");

                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Validando credenciales...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                var response = await IdentityService.Login(EntEmail.Text, EntPassword.Text);
                
                if (!response)
                    throw new Exception("Las credenciales de acceso no son validas.");

                await Navigation.PopPopupAsync(true);

                Application.Current.MainPage = new NavigationPage(new HomePage());

                await Navigation.PopAsync();
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

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }

        //private void BtnClear_Clicked(object sender, EventArgs e)
        //{
        //    EntEmail.Text = string.Empty;
        //    EntPassword.Text = string.Empty;
        //}
    }
}