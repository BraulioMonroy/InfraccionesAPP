using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.CashClosing
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CashClosingSentSuccessPopupView : PopupPage
    {
        public CashClosingSentSuccessPopupView()
        {
            InitializeComponent();
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