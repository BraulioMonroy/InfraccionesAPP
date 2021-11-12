using System;

namespace Infracciones.Models
{
    public class LogModel
    {
        public int Id { get; set; }
        public string Module { get; set; }
        public string Changes { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Ip { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
