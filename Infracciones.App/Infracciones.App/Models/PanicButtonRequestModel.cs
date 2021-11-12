namespace Infracciones.Models
{
    public class PanicButtonRequestModel
    {         
        public int OriginId { get; set; }
        public string Description { get; set; }
        public int MotiveId { get; set; }
        public int AddressId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int MobileUuid { get; set; }
        public int Uuid { get; set; }
        public string Priority { get; set; }
        public string OriginIntegration { get; set; }
        public PanicButtonInformer Informer { get; set; }
        public string CreatedBy { get; set; }
        public LogModel Log { get; set; }
    }

    public class PanicButtonInformer
    {
        public string GuardaDirectorio { get; set; } = string.Empty;
        public string IdEvento { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
        public string tipoDenunciante { get; set; } = string.Empty;
        public string uuid { get; set; } = "5";
    }
}
