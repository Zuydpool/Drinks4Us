using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryFridgeDao : IFridgeDao
    {
        private readonly IDictionary<int, Fridge> _items = new ConcurrentDictionary<int, Fridge>();

        public Task<Fridge> GetById(int id)
        {
            return Task.Factory.StartNew(() => _items[id]);
        }

        public Task<ICollection<Fridge>> GetAll()
        {
            return Task.Factory.StartNew(() => _items.Values);
        }

        public Task<bool> Delete(int id)
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