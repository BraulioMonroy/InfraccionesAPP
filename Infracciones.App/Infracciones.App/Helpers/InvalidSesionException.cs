using Infracciones.App.Views.Identity;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Infracciones.Helpers
{
    public class InvalidSesionException : Exception
    {
        public InvalidSesionException()
        {
            CloseAllPopup();

            var errorPopupView = new ErrorPopupView()
            {
                BindingContext = new PopupViewModel()
                {
                    Text = "La sesión expiró.",
                    Image = "Error.png"
                }
            };

            Task.Run(async () => await Application.Current.MainPage.Navigation.PushPopupAsync(errorPopupView));
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public InvalidSesionException(string message)
            : base(message)
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

            Task.Run(async () => await Application.Current.MainPage.Navigation.PushPopupAsync(errorPopupView));
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public InvalidSesionException(string message, Exception innerException)
            : base(message, innerException)
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

            Task.Run(async () => await Application.Current.MainPage.Navigation.PushPopupAsync(errorPopupView));
            Task.Run(async () => await Task.Delay(1000));
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
