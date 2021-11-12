using Newtonsoft.Json;

namespace Infracciones.App.Models
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        //public int? ExpiresIn { get; set; }
        //public string TokenType { get; set; }
        public int? CreationTime { get; set; }
        public int? ExpirationTime { get; set; }
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        public string Key { get; set; }
        public int? SectorId { get; set; }
        public string Sector { get; set; }
        public int? ShiftId { get; set; }
        public string Shift { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
