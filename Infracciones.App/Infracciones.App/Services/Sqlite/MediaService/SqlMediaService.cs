using Infracciones.Helpers;
using Infracciones.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Infracciones.Services.Sqlite.MediaService
{
    public class SqlMediaService : ISqlMediaService
    {
        private static readonly AsyncLock Mutex = new AsyncLock();
        private SQLiteAsyncConnection _sqlCon;

        public SqlMediaService()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            _sqlCon = new SQLiteAsyncConnection(databasePath);

            CreateDatabaseAsync();
        }

        public async void CreateDatabaseAsync()
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                await _sqlCon.CreateTableAsync<MediaModel>().ConfigureAwait(false);
            }
        }

        public async Task<IList<MediaModel>> GetAll()
        {
            var items = new List<MediaModel>();
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                items = await _sqlCon.Table<MediaModel>()
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            return items;
        }

        public async Task<IList<MediaModel>> GetAllBySantionReason(SanctionReason sanctionReason)
        {
            var items = new List<MediaModel>();
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                items = await _sqlCon.Table<MediaModel>()
                    .Where(o => o.SanctionReasonId == (int)sanctionReason)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            return items;
        }

        public async Task<IList<MediaModel>> GetAllVideosAndImages(SanctionReason sanctionReason)
        {
            var items = new List<MediaModel>();
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                items = await _sqlCon.Table<MediaModel>()
                    .Where(o => o.SanctionReasonId == (int)sanctionReason && o.MediaType.Equals("Image") || o.MediaType.Equals("Video"))
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            return items;
        }

        public async Task<IList<MediaModel>> GetImages(SanctionReason sanctionReason)
        {
            var items = new List<MediaModel>();
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                items = await _sqlCon.Table<MediaModel>()
                    .Where(o => o.SanctionReasonId == (int)sanctionReason && o.MediaType.Equals("Image"))
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            return items;
        }

        public async Task<IList<MediaModel>> GetVideos(SanctionReason sanctionReason)
        {
            var items = new List<MediaModel>();
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                items = await _sqlCon.Table<MediaModel>()
                    .Where(o => o.SanctionReasonId == (int)sanctionReason && o.MediaType.Equals("Video"))
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            return items;
        }

        public async Task Insert(MediaModel item)
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                var existingImage = await _sqlCon.Table<MediaModel>()
                        .Where(x => x.Id == item.Id)
                        .FirstOrDefaultAsync();

                if (existingImage == null)
                {
                    await _sqlCon.InsertAsync(item).ConfigureAwait(false);
                }
                else
                {
                    item.Id = existingImage.Id;
                    await _sqlCon.UpdateAsync(item).ConfigureAwait(false);
                }
            }
        }

        public async Task<MediaModel> GetById(int id)
        {
            var item = new MediaModel();
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                item = await _sqlCon.Table<MediaModel>()
                    .FirstOrDefaultAsync(o => o.Id == id)
                    .ConfigureAwait(false);
            }

            return item;
        }

        public async Task Remove(MediaModel item)
        {
            await _sqlCon.DeleteAsync(item);
        }

        public async Task RemoveAll()
        {
            var items = await GetAll();

            foreach (var item in items)
            {
                await _sqlCon.DeleteAsync(item);
            }
        }
    }
}
