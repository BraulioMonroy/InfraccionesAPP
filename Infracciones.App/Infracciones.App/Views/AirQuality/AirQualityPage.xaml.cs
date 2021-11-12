using Infracciones.App.Views.Home;
using Infracciones.App.Views.Identity;
using Infracciones.App.Views.Shared;
using Infracciones.Services.Geolocation;
using Infracciones.ViewModels.Shared;
using Infracciones.Views.AirQuality;
using Infracciones.Views.PanicButton;
using Infracciones.Views.Shared;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.App.Views.AirQuality
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AirQualityPage : ContentPage
    {
        public AirQualityPage()
        {
            InitializeComponent();

            //TODO: FIX Error here
            //ImageDummy1Container.ImageToSize = string.Concat(App.BaseImageUrl, "Assets/aire_01.png");
            //ImageDummy1.Source = new UriImageSource
            //{
            //    Uri = new Uri(string.Concat(App.BaseImageUrl, "Assets/aire_01.png")),
            //    CachingEnabled = true,
            //    CacheValidity = new TimeSpan(7, 0, 0, 0)
            //};

            //ImageDummy2Container.ImageToSize = string.Concat(App.BaseImageUrl, "Assets/aire_02.png");
            //ImageDummy2.Source = new UriImageSource
            //{
            //    Uri = new Uri(string.Concat(App.BaseImageUrl, "Assets/aire_02.png")),
            //    CachingEnabled = true,
            //    CacheValidity = new TimeSpan(7, 0, 0, 0)
            //};
            //Adding temp images
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

        private async void Locate_Tapped(object sender, EventArgs e)
        {
            try
            {
                var loading = new LoadingPopupView
                {
                    BindingContext = new PopupViewModel()
                    {
                        Text = "Cargando...",
                        TextStyle = "PopupContentLabelStyle"
                    }
                };

                await Navigation.PushPopupAsync(loading);

                var geolocationService = new GeolocationService();
                var currentLocation = await geolocationService.GetFullCurrentLocation();

                await Navigation.PushModalAsync(new MapPage(currentLocation.Latitude, currentLocation.Longitude));
                CloseAllPopup();
            }
            catch (Exception x)
            {
                await DisplayAlert("Error", x.Message, "Aceptar");
            }
        }

        private async void CloseAllPopup()
        {
            var popups = PopupNavigation.Instance.PopupStack;

            if (popups.Count > 0)
                await PopupNavigation.Instance.PopAllAsync();
        }
    }
}