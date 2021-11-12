using Infracciones.App.Views.Home;
using Infracciones.Views.PanicButton;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.Repuve
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepuveFoundPage : ContentPage
    {
        public RepuveFoundPage()
        {
            InitializeComponent();

            //TODO: FIXBUG HERE
            //ImageDummyRepuveContainer.ImageToSize = string.Concat(App.BaseImageUrl, "Assets/repuve_result.png");
            //ImageDummyRepuve.Source = new UriImageSource
            //{
            //    Uri = new Uri(string.Concat(App.BaseImageUrl, "Assets/repuve_result.png")),
            //    CachingEnabled = true,
            //    CacheValidity = new TimeSpan(7, 0, 0, 0)
            //};

            //ADDING TEMP IMAGE
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
    }
}