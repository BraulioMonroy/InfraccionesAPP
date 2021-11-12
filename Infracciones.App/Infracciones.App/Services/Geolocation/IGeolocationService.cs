using System.Threading.Tasks;

namespace Infracciones.Services.Geolocation
{
    public interface IGeolocationService
    {
        Task<string> GetLocationAsync();
        Task<string> GetLastKnownLocationAsync();
        Task<string> GetCurrentLocation();
    }
}
