namespace Drinks4Us.Storage.Dao.SQLite
{
    public class SQLiteDao : IStorageDao
    {
        public IFridgeItemsDao FridgeItemsDao { get; }

        public IAppUsersDao AppUsersDao { get; }

        public IFridgeDao FridgeDao { get; }
    }
}