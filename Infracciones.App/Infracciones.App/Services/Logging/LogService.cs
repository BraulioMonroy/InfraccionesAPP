using Infracciones.Models;
using Infracciones.Services.Geolocation;
using Infracciones.Services.Network;
using System;
using System.Threading.Tasks;

namespace Infracciones.Services.Logging
{
    public class LogService : ILogService
    {
        private static readonly IPService _ipService = new IPService();
        private static readonly GeolocationService _geolocationService = new GeolocationService();

        public async Task<LogModel> CreateLog()
        {
            var coordinates = await GetLocation();

            var log = new LogModel()
            {
                Client = "App",
                Ip = GetIpAddress(),
                Latitude = (coordinates != null) ? coordinates.Latitude.ToString() : string.Empty,
                Longitude = (coordinates != null) ? coordinates.Longitude.ToString() : string.Empty,
            };

            return log;
        }

        private string GetIpAddress()
        {
            try
            {
                return _ipService.GetCurrentIPAddress();
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }

        private async Task<Xamarin.Essentials.Location> GetLocation()
        {
            try
            {
                return await _geolocationService.GetFullCurrentLocation();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
