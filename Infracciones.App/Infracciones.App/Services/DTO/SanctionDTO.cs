using Infracciones.App.Models;
using System.Collections.Generic;

namespace Infracciones.App.Services.DTO
{
    public class SanctionDTO
    {
        public VehicleModel Vehicle { get; set; }
        public List<SanctionReasonModel> SanctionReasons { get; set; }
    }
}
