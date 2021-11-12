using Infracciones.Models;

namespace Infracciones.App.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LogModel Log { get; set; }
    }
}
