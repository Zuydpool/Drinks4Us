namespace Drinks4Us.Storage.Dao
{
    public interface IStorageDao
    {
        IFridgeItemsDao FridgeItemsDao { get; }

        IAppUsersDao AppUsersDao { get; }

        IFridgeDao FridgeDao { get; }
    }
}