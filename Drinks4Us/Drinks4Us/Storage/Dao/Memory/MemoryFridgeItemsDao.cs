using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryFridgeItemsDao : IFridgeItemsDao
    {
        private readonly IDictionary<string, FridgeItem> _items = new ConcurrentDictionary<string, FridgeItem>();

        public MemoryFridgeItemsDao()
        {
            var id = Guid.NewGuid().ToString();
            _items.Add(id, new FridgeItem
            {
                Id = id,
                Name = "1.5L Cola",
                Quantity = 5,
                PurchaseDate = DateTime.Today.AddDays(-14),
                ExpireDate = DateTime.Today.AddDays(-2),
                ImageUrl = "https://www.compliment.nl/wp-content/uploads/2022/10/10030.jpg"
            });
        }

        public Task<FridgeItem> GetById(string id)
        {
            return Task.Factory.StartNew(() => _items[id]);
        }

        public Task<ICollection<FridgeItem>> GetAll()
        {
            return Task.Factory.StartNew(() => _items.Values);
        }

        public Task<bool> Delete(string id)
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