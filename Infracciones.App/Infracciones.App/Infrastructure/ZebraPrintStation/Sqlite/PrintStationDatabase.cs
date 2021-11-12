using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.Infrastructure.ZebraPrintStation.Sqlite
{
    public class PrintStationDatabase
    {

        public const string StoredFormatTableName = "StoredFormat";

        private SQLiteAsyncConnection database;

        public PrintStationDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<StoredFormat>().Wait();
        }

        public async Task<StoredFormat> GetStoredFormatAsync(int id)
        {
            return await database.Table<StoredFormat>().Where(format => format.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<StoredFormat>> GetStoredFormatsAsync()
        {
            string query = $"SELECT * FROM {StoredFormatTableName} ORDER BY {nameof(StoredFormat.DriveLetter)} ASC, {nameof(StoredFormat.Name)} ASC, {nameof(StoredFormat.Extension)} ASC";
            return await database.QueryAsync<StoredFormat>(query);
        }

        public async Task<List<StoredFormat>> GetStoredFormatsWithSamePrinterPathAsync(StoredFormat storedFormat)
        {
            string query = $"SELECT * FROM {StoredFormatTableName} WHERE {nameof(StoredFormat.DriveLetter)} = ? AND {nameof(StoredFormat.Name)} = ? AND {nameof(StoredFormat.Extension)} = ? ORDER BY {nameof(StoredFormat.DriveLetter)} ASC, {nameof(StoredFormat.Name)} ASC, {nameof(StoredFormat.Extension)} ASC";
            return await database.QueryAsync<StoredFormat>(query, storedFormat.DriveLetter, storedFormat.Name, storedFormat.Extension);
        }

        public async Task<int> SaveStoredFormatAsync(StoredFormat storedFormat)
        {
            List<StoredFormat> formats = await GetStoredFormatsWithSamePrinterPathAsync(storedFormat);

            foreach (StoredFormat format in formats)
            {
                await DeleteStoredFormatByIdAsync(format.Id);
            }

            if (storedFormat.Id != 0)
            {
                return await database.UpdateAsync(storedFormat);
            }
            else
            {
                return await database.InsertAsync(storedFormat);
            }
        }

        public async Task<int> DeleteStoredFormatByIdAsync(int id)
        {
            return await database.DeleteAsync(await GetStoredFormatAsync(id));
        }

        [Table(StoredFormatTableName)]
        public class StoredFormat
        {
            [PrimaryKey, AutoIncrement, Column("_id")]
            public int Id { get; set; }

            [NotNull, MaxLength(1)]
            public string DriveLetter { get; set; }

            [NotNull]
            public string Name { get; set; }

            [NotNull]
            public string Extension { get; set; }

            public string Content { get; set; }

            public string PrinterPath
            {
                get => $"{DriveLetter}:{Name}.{Extension}";
            }
        }
    }
}
