using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopupView : PopupPage
    {
        public LoadingPopupView()
        {
            InitializeComponent();
        }
    }
}