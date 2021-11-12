using System.Net;

namespace Infracciones.Services.Network
{
    public class IPService : IIPService
    {
        public string GetCurrentIPAddress()
        {
            var ipAddress = string.Empty;

            foreach (IPAddress adress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                ipAddress =  adress.ToString();
                break;
            }

            return ipAddress;
        }
    }
}
