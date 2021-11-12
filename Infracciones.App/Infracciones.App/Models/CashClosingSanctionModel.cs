using System;

namespace Infracciones.App.Models
{
    public class CashClosingSanctionModel
    {
        public int Id { get; set; }
        public int CashClosingId { get; set; }
        public int SanctionId { get; set; }
        public bool Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
