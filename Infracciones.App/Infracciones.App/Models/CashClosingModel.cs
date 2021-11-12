using Infracciones.Models;
using System;
using System.Collections.Generic;

namespace Infracciones.App.Models
{
    public class CashClosingModel
    {
        public int Id { get; set; }
        public string OfficerId { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public bool Enabled { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ShiftModel Shift { get; set; }
        public IEnumerable<CashClosingSanctionModel> CashClosingSanctions { get; set; }

        public LogModel Log { get; set; }
    }
}
