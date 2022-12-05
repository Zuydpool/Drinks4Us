using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryFridgeItemsDao : IFridgeItemsDao
    {
        private readonly IDictionary<int, FridgeItem> _items = new ConcurrentDictionary<int, FridgeItem>();

        public Task<FridgeItem> GetById(int id)
        {
            return Task.Factory.StartNew(() => _items[id]);
        }

        public Task<ICollection<FridgeItem>> GetAll()
        {
            return Task.Factory.StartNew(() => _items.Values);
        }

        public Task<bool> Delete(int id)
        {
            return Task.Factory.StartNew(() => _items.Remove(id));
        }

        public Task<FridgeItem> Add(FridgeItem entry)
        {
            _items.Add(entry.Id, entry);
            return Task.Factory.StartNew(() => entry);
        }

        public Task<FridgeItem> Update(FridgeItem entry)
        {
            _items[entry.Id] = entry;
            return Task.Factory.StartNew(() => entry);
        }
    }
}