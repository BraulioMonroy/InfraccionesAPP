using Infracciones.App.Views.Sanctions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Vehicle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SanctionsPopupView : PopupPage
    {
        public SanctionsPopupView()
        {
            InitializeComponent();
        }

        private async void BtnMakeSanctionPopup_Clicked(object sender, EventArgs e)
        {
            CloseAllPopup();

            var sanctionPage = new SanctionPage();
            sanctionPage.BindingContext = this.BindingContext;
            await Navigation.PushAsync(sanctionPage, true);
        }

        private void BtnCancelPopup_Clicked(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}