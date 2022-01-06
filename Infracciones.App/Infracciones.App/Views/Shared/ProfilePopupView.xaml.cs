using Infracciones.App.Services;
using Infracciones.App.Views.Identity;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePopupView : PopupPage
    {
        public ProfilePopupView()
        {
            InitializeComponent();
            //var culture = new CultureInfo("es-mx");
            //CurrentDateLabel.Text = DateTime.Now.ToString("D", culture);
            //CurrentTimeLabel.Text = DateTime.Now.ToString("HH:mm");
        }

        private async void ToggleButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void TapLogout_Tapped(object sender, EventArgs e)
        {
            IdentityService.Logout();
            await PopupNavigation.Instance.PopAsync(true);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            var editProfilePopupView = new EditProfilePopupView();
            await Navigation.PushPopupAsync(editProfilePopupView);
        }
    }
}