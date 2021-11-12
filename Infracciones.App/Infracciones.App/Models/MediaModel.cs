using SQLite;

namespace Infracciones.Models
{
    public class MediaModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string OfficialId { get; set; }     
        public string ImagePath { get; set; }
        public byte[] Image { get; set; }
        public int SanctionReasonId { get; set; }
        public string MediaType { get; set; } //Video, Image       
    }
}
