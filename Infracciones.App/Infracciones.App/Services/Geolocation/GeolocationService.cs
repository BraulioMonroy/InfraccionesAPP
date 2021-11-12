using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Infracciones.Services.Geolocation
{
    public class GeolocationService : IGeolocationService
    {

        public async Task<Location> GetFullCurrentLocation()
        {
            try
            {
                var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Xamarin.Essentials.Geolocation.GetLocationAsync(new Xamarin.Essentials.GeolocationRequest()
                    {
                        DesiredAccuracy = Xamarin.Essentials.GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(15)
                    });
                }

                return location;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<string> GetCurrentLocation()
        {
            try
            {
                var currentLocation = string.Empty;

                var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Xamarin.Essentials.Geolocation.GetLocationAsync(new Xamarin.Essentials.GeolocationRequest()
                    {
                        DesiredAccuracy = Xamarin.Essentials.GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(15)
                    });                   
                }

                if (location != null)
                    currentLocation = string.Concat(location.Latitude, ",", location.Longitude);

                return currentLocation;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> GetLastKnownLocationAsync()
        {
            try
            {
                var currentLocation = string.Empty;

                var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                    currentLocation = string.Concat(location.Latitude, ",", location.Longitude);

                return currentLocation;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> GetLocationAsync()
        {
            try
            {
                var currentLocation = string.Empty;

                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium);
                var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);

                if (location != null)
                    currentLocation = string.Concat(location.Latitude, ",", location.Longitude);

                return currentLocation;
            }
            catch (Exception)
            {
                return string.Empty;
            }           
        }
    }
}
