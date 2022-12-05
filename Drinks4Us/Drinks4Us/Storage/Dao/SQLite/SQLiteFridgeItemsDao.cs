using System.Collections.Generic;
using System.Threading.Tasks;
using Drinks4Us.Models;
using SQLiteNetExtensionsAsync.Extensions;

namespace Drinks4Us.Storage.Dao.SQLite
{
    public class SQLiteFridgeItemsDao : IFridgeItemsDao
    {

        private readonly SQLiteStorage storage;
        public SQLiteFridgeItemsDao(SQLiteStorage storage)
        {
            this.storage = storage;
        }

        public async Task<FridgeItem?> GetById(int id)
        {
            return await storage.GetAsyncConnection().GetWithChildrenAsync<FridgeItem>(id);
        }

        public async Task<ICollection<FridgeItem>> GetAll()
        {
            var result = await storage.GetAsyncConnection().Table<FridgeItem>().ToListAsync() ?? new List<FridgeItem>();

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await storage.GetAsyncConnection().DeleteAsync<FridgeItem>(id).ContinueWith((t) => t.IsCompleted);
        }

        public async Task<FridgeItem> Add(FridgeItem entry)
        {
            return await storage.GetAsyncConnection().InsertAsync(entry).ContinueWith((_) => entry);
        }

        public async Task<FridgeItem> Update(FridgeItem entry)
        {
            return await storage.GetAsyncConnection().UpdateAsync(entry).ContinueWith((_) => entry);
        }
    }
}