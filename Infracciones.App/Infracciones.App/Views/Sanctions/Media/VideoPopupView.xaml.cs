using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Sanctions.Media
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPopupView : PopupPage
    {
        public VideoPopupView()
        {
            InitializeComponent();
        }

        private async void ButtonCanel_Clicked(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}