using Infracciones.App.Views.Home;
using Infracciones.Views.PanicButton;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Payment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintedSanctionsPayedPage : ContentPage
    {
        public PrintedSanctionsPayedPage()
        {
            InitializeComponent();
            ReturnToHome();
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void ReturnToHome()
        {
            await Task.Delay(4000);
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }
    }
}