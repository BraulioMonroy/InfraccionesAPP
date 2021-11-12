using System.Collections.Generic;

namespace Infracciones.App.Models
{
    public class VehicleModel
    {
        public string Plates { get; set; }
        public string Responsible { get; set; }
        public BrandModel Brand { get; set; }
        public SubBrandModel Subbrand { get; set; }
        public int Model { get; set; }
        public StateModel Origin { get; set; }
        public VehicleTypeModel VehicleType { get; set; }
        public bool CheckRepuve { get; set; }
        public string CheckRepuveString { get; set; }
        public List<SanctionModel> Sanctions { get; set; }
    }
}
