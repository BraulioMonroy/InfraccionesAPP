using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infracciones.Views.AirQuality
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage(double latitude, double longitude)
        {
            InitializeComponent();
            
            map.MoveToRegion(new Xamarin.Forms.Maps.MapSpan(new Xamarin.Forms.Maps.Position(latitude, longitude), 0.01, 0.01));
        }

        private async void map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            await DisplayAlert("Coords", $"Lat: {e.Position.Latitude} \n Lon: {e.Position.Longitude}", "Aceptar");
        }
    }
}