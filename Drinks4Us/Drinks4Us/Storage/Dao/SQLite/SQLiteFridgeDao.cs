using System.Collections.Generic;
using System.Threading.Tasks;
using Drinks4Us.Models;
using SQLiteNetExtensionsAsync.Extensions;

namespace Drinks4Us.Storage.Dao.SQLite
{
    public class SQLiteFridgeDao : IFridgeDao
    {
        private readonly SQLiteStorage storage;
        public SQLiteFridgeDao(SQLiteStorage storage)
        {
            this.storage = storage;
        }

        public async Task<Fridge?> GetById(int id)
        {
            return await storage.GetAsyncConnection().GetWithChildrenAsync<Fridge>(id);
        }

        public async Task<ICollection<Fridge>> GetAll()
        {
            var result = await storage.GetAsyncConnection().GetAllWithChildrenAsync<Fridge>() ?? new List<Fridge>();

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await storage.GetAsyncConnection().DeleteAsync<Fridge>(id).ContinueWith((t) => t.IsCompleted);
        }

        public async Task<Fridge> Add(Fridge entry)
        {
            return await storage.GetAsyncConnection().InsertAsync(entry).ContinueWith((_) => entry);
        }

        public async Task<Fridge> Update(Fridge entry)
        {
            return await storage.GetAsyncConnection().UpdateWithChildrenAsync(entry).ContinueWith((_) => entry);
        }
    }
}