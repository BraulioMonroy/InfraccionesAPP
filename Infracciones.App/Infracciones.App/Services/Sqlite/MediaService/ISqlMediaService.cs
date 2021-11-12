using Infracciones.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.Services.Sqlite.MediaService
{
    public interface ISqlMediaService
    {
        Task<IList<MediaModel>> GetAll();
        Task<IList<MediaModel>> GetAllBySantionReason(SanctionReason sanctionReason);
        Task<IList<MediaModel>> GetImages(SanctionReason sanctionReason);
        Task<IList<MediaModel>> GetVideos(SanctionReason sanctionReason);
        Task<MediaModel> GetById(int id);
        Task Insert(MediaModel item);
        Task Remove(MediaModel item);
        Task RemoveAll();
    }
}
