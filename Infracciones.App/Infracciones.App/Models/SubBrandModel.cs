using System;

namespace Infracciones.App.Models
{
    public class SubBrandModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public bool Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
