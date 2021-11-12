using System;

namespace Infracciones.Services.DTO
{
    public class PaymentDTO
    {
        public int SanctionId { get; set; }
        public DateTime CreationDate { get; set; }
        public string PaymentReference { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Voucher { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Card { get; set; }
        public string TransactionNumber { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Closed { get; set; }
        public DateTime? DateClosed { get; set; }
        public string ClosedBy { get; set; }
    }
}
