using Drinks4Us.Storage.Dao;

namespace Drinks4Us.Storage
{
    public interface IStorage
    {
        IStorageDao Dao { get; }

        void SetupStorage();
    }
}