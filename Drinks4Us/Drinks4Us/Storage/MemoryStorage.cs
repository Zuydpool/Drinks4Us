using System;
using System.Collections.Generic;
using Drinks4Us.Models;
using Drinks4Us.Storage.Dao;
using Drinks4Us.Storage.Dao.Memory;

namespace Drinks4Us.Storage
{
    public class MemoryStorage : IStorage
    {
        public IStorageDao Dao { get; } = new MemoryDao();

        public void SetupStorage()
        {
            InsertFakeData();
        }

        void InsertFakeData()
        {
            var fridgeItem = new FridgeItem
            {
                Id = 1,
                Name = "1.5L Cola",
                Quantity = 5,
                PurchaseDate = DateTime.Today.AddDays(-14),
                ExpireDate = DateTime.Today.AddDays(2),
                ImageUrl = "https://www.compliment.nl/wp-content/uploads/2022/10/10030.jpg"
            };

            var fridge = new Fridge()
            {
                Id = 1,
                Name = "Samsung Big Americans Koelkast jonguh",
                ImageUrl =
                    "https://dehanzewitgoed.nl/wp-content/uploads/2020/10/Samsung-RS68N8321S9-Amerikaanse-koelkast.jpg",
                FridgeItems = new List<FridgeItem>() { fridgeItem }
            };
            fridgeItem.Fridge = fridge;

            Dao.FridgeItemsDao.Add(fridgeItem);
            Dao.FridgeDao.Add(fridge);
        }
    }
}