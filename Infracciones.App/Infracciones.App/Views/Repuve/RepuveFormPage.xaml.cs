using Infracciones.App.Views.Home;
using Infracciones.App.Views.Identity;
using Infracciones.App.Views.Shared;
using Infracciones.ViewModels.Repuve;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Repuve
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepuveFormPage : ContentPage
    {
        public RepuveFormPage()
        {
            InitializeComponent();

            BindingContext = new RepuveFormViewModel();
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            ErrorLabel.Text = string.Empty;

            //// Searching popup view
            //var searchingPopupView = new RepuveSearchingPopupView();
            //await Navigation.PushPopupAsync(searchingPopupView);

            //// Delay Mock
            //await Task.Delay(2000);
            //await Navigation.RemovePopupPageAsync(searchingPopupView);

            var loading = new LoadingPopupView
            {
                BindingContext = new PopupViewModel()
                {
                    Text = "Cargando...",
                    TextStyle = "PopupContentLabelStyle"
                }
            };

            await Navigation.PushPopupAsync(loading);
            await Task.Delay(2000);

            // Push found page
            await Navigation.PushAsync(new RepuveFoundPage(), true);
            CloseAllPopup();
        }

        private async void PanicButton_Clicked(object sender, EventArgs e)
        {
            var popupView = new PanicButtonPopupView();
            await Navigation.PushPopupAsync(popupView, true);
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new HomePage(), this);
            await Navigation.PopToRootAsync(true);
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }
    }
}