using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryFridgeDao : IFridgeDao
    {
        private readonly IDictionary<string, Fridge> _items = new ConcurrentDictionary<string, Fridge>();

        public MemoryFridgeDao()
        {
            var id = Guid.NewGuid().ToString();
            _items.Add(id, new Fridge()
            {
                Id = id,
                Name = "Samsung Big Americans Koelkast jonguh",
                ImageUrl = "https://dehanzewitgoed.nl/wp-content/uploads/2020/10/Samsung-RS68N8321S9-Amerikaanse-koelkast.jpg"
            });
        }

        public Task<Fridge> GetById(string id)
        {
            return Task.Factory.StartNew(() => _items[id]);
        }

        public Task<ICollection<Fridge>> GetAll()
        {
            return Task.Factory.StartNew(() => _items.Values);
        }

        public Task<bool> Delete(string id)
        {
            return Task.Factory.StartNew(() => _items.Remove(id));
        }

        public Task<Fridge> Add(Fridge entry)
        {
            _items.Add(entry.Id, entry);
            return Task.Factory.StartNew(() => entry);
        }

        public Task<Fridge> Update(Fridge entry)
        {
            _items[entry.Id] = entry;
            return Task.Factory.StartNew(() => entry);
        }
    }
}