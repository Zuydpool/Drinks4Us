using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryLogDao : ILogDao
    {
        private readonly IDictionary<int, Log> _items = new ConcurrentDictionary<int, Log>();

        public Task<Log?> GetById(int id)
        {
            return Task.FromResult(_items[id]);
        }

        public Task<ICollection<Log>> GetAll()
        {
            return Task.FromResult(_items.Values);
        }

        public Task<bool> Delete(int id)
        {
            return Task.Factory.StartNew(() => _items.Remove(id));
        }

        public Task<Log> Add(Log entry)
        {
            _items.Add(entry.Id, entry);
            return Task.Factory.StartNew(() => entry);
        }

        public Task<Log> Update(Log entry)
        {
            _items[entry.Id] = entry;
            return Task.Factory.StartNew(() => entry);
        }
    }
}