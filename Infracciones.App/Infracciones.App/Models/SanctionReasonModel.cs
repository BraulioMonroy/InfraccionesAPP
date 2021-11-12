using Infracciones.Models;
using System;

namespace Infracciones.App.Models
{
    public class SanctionReasonModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal SalaryQuantity { get; set; }
        public string IconPath { get; set; }
        public bool Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }

        public LogModel Log { get; set; }
    }
}
