using Infracciones.Models;
using System;

namespace Infracciones.App.Models
{
    public class SanctionModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Plates { get; set; }
        public int Model { get; set; }
        public string Responsible { get; set; }
        public string ResponsibleEmail { get; set; }
        public int VehicleTypeId { get; set; }
        public int BrandId { get; set; }
        public int SubBrandId { get; set; }
        public int ColorId { get; set; }
        public int Origin { get; set; }
        public int SanctionReasonId { get; set; }
        public string OfficerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual VehicleTypeModel VehicleType { get; set; }
        public virtual BrandModel Brand { get; set; }
        public virtual SubBrandModel SubBrand { get; set; }
        public virtual ColorModel Color { get; set; }
        public virtual StateModel State { get; set; }
        public virtual SanctionReasonModel SanctionReason { get; set; }
        public PaymentModel Payment { get; set; }

        public LogModel Log { get; set; }
    }
}
