using Infracciones.Models;

namespace Infracciones.Services.DTO
{
    public class RecoverPasswordDTO
    {
        public string Email { get; set; }

        public LogModel Log { get; set; }
    }
}
