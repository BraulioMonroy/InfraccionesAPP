using Newtonsoft.Json;

namespace Infracciones.Models
{
    public class PanicButtonResponseModel
    {
        [JsonProperty(PropertyName = "cuenta")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "mensaje")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "motivo")]
        public string Motive { get; set; }

        [JsonProperty(PropertyName = "origen")]
        public string Origin { get; set; }

        [JsonProperty(PropertyName = "prioridad")]
        public string Priority { get; set; }

        [JsonProperty(PropertyName = "v_fecha_evento")]
        public string DateEvent { get; set; }

        [JsonProperty(PropertyName = "v_folio")]
        public string SerialKey { get; set; }

        [JsonProperty(PropertyName = "v_id_evento")]
        public string EventId { get; set; }

        [JsonProperty(PropertyName = "v_institucion_asignada")]
        public string AssignedInstitution { get; set; }

        [JsonProperty(PropertyName = "v_uuid")]
        public string UUID { get; set; }
    }
}
