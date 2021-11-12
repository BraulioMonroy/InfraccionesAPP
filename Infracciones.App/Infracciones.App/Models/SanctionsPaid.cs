using System;

namespace Infracciones.App.Models
{
    public class SanctionsPaid
    {
        public int SanctionId { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Plates { get; set; }
        public string Officer { get; set; }
        public string Amount { get; set; }
    }
}
