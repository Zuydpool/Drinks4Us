namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryDao : IStorageDao
    {
        public IFridgeItemsDao FridgeItemsDao { get; } = new MemoryFridgeItemsDao();

        public IAppUsersDao AppUsersDao { get; } = new MemoryAppUsersDao();

        public IFridgeDao FridgeDao { get; } = new MemoryFridgeDao();

        public ILogDao LogDao { get; } = new MemoryLogDao();
    }
}