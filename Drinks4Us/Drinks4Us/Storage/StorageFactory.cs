using System;

namespace Drinks4Us.Storage
{
    public class StorageFactory
    {
        public IStorage GetInstance()
        {
            var storage = CreateNewImplementation(StorageType.SQLITE);
            storage.SetupStorage();
            return storage;
        }

        public IStorage CreateNewImplementation(StorageType storageType)
        {
            return storageType switch
            {
                StorageType.MEMORY => new MemoryStorage(),
                StorageType.SQLITE => new SQLiteStorage(),
                _ => throw new Exception("Unknown storage type " + storageType)
            };
        }
    }
}