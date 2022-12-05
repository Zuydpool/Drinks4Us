namespace Drinks4Us.Storage.Dao.SQLite
{
    public class SQLiteDao : IStorageDao
    {

        public SQLiteDao(SQLiteStorage storage)
        {
            AppUsersDao = new SQLiteAppUsersDao(storage);
            FridgeDao = new SQLiteFridgeDao(storage);
            FridgeItemsDao = new SQLiteFridgeItemsDao(storage);
            LogDao = new SQLiteLogDao(storage);
        }

        public IFridgeItemsDao FridgeItemsDao { get; }

        public IAppUsersDao AppUsersDao { get; }

        public IFridgeDao FridgeDao { get; }

        public ILogDao LogDao { get; }
    }
}