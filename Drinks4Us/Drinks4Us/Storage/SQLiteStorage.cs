using Drinks4Us.Storage.Dao.Memory;
using Drinks4Us.Storage.Dao;
using Drinks4Us.Storage.Dao.SQLite;

namespace Drinks4Us.Storage
{
    public class SQLiteStorage : IStorage
    {
        public IStorageDao Dao { get; } = new SQLiteDao();

        public void SetupStorage()
        {
            
        }
    }
}