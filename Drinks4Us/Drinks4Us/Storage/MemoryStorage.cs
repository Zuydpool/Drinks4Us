using Drinks4Us.Storage.Dao;
using Drinks4Us.Storage.Dao.Memory;

namespace Drinks4Us.Storage
{
    public class MemoryStorage : IStorage
    {
        public IStorageDao Dao { get; } = new MemoryDao();

        public void SetupStorage()
        {
            
        }
    }
}