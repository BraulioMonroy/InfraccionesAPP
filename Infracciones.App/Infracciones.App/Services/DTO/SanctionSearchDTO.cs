using Infracciones.Models;
using System;

namespace Infracciones.App.Services.DTO
{
    public class SanctionSearchDTO
    {
        public bool TodaySanctions { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Plates { get; set; }
        public string Officer { get; set; }
        public string Status { get; set; }
        public int? Sector { get; set; }
        public int? Origin { get; set; }
        public int? SanctionReason { get; set; }
        public int DeferredDays { get; set; }
        public int? Color { get; set; }
        public int? Brand { get; set; }
        public int? Model { get; set; }
        public int? SubBrand { get; set; }
        public int? VehicleType { get; set; }

        public LogModel Log { get; set; }
    }
}
