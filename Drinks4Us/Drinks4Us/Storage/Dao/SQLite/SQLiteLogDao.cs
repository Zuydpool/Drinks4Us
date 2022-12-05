using Drinks4Us.Models;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drinks4Us.Storage.Dao.SQLite
{
    public class SQLiteLogDao : ILogDao
    {
        private readonly SQLiteStorage storage;
        public SQLiteLogDao(SQLiteStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Log?> GetById(int id)
        {
            return await storage.GetAsyncConnection().GetWithChildrenAsync<Log>(id);
        }

        public async Task<ICollection<Log>> GetAll()
        {
            var result = await storage.GetAsyncConnection().Table<Log>().ToListAsync() ?? new List<Log>();

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await storage.GetAsyncConnection().DeleteAsync<Log>(id).ContinueWith((t) => t.IsCompleted);
        }

        public async Task<Log> Add(Log entry)
        {
            return await storage.GetAsyncConnection().InsertAsync(entry).ContinueWith((_) => entry);
        }

        public async Task<Log> Update(Log entry)
        {
            return await storage.GetAsyncConnection().UpdateAsync(entry).ContinueWith((_) => entry);
        }
    }
}