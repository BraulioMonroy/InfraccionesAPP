using Infracciones.Models;
using System;
using System.Collections.Generic;

namespace Infracciones.App.Models
{
    public class CashClosingDtoModel
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
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public IEnumerable<PaymentModel> Payments { get; set; }
        public LogModel Log { get; set; }
    }
}
