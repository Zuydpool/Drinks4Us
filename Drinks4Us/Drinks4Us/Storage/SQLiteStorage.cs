using System.Diagnostics;
using Drinks4Us.Models;
using Drinks4Us.Storage.Dao;
using Drinks4Us.Storage.Dao.SQLite;
using SQLite;

namespace Drinks4Us.Storage
{
    public class SQLiteStorage : IStorage
    {
        public IStorageDao Dao { get; }

        public string DatabaseFilePath { get; set; } = string.Empty;

        public SQLiteStorage()
        {
            Dao = new SQLiteDao(this);
        }

        public void SetupStorage()
        {
            Debug.WriteLine(App.GetInstance().SQLiteDatabaseLocation);
            DatabaseFilePath = App.GetInstance().SQLiteDatabaseLocation;

            CreateTables();
        }

        public void CreateTables()
        {
            using var connection = GetConnection();
            connection.CreateTable<Fridge>();
            connection.CreateTable<FridgeItem>();
            connection.CreateTable<AppUser>();
            connection.CreateTable<Log>();
            /*var createAppUsersTableSql =
                @"CREATE TABLE IF NOT EXISTS app_users(id TEXT PRIMARY KEY, email VARCHAR(253) NOT NULL, password TEXT NOT NULL);";

            var createFridgesTableSql = @"CREATE TABLE IF NOT EXISTS fridges(id TEXT PRIMARY KEY, name VARCHAR(64) NOT NULL, image_url TEXT, last_filled_by TEXT);";

            var createFridgeItemsTableSql = @"CREATE TABLE IF NOT EXISTS fridge_items(id TEXT PRIMARY KEY, name VARCHAR(64) NOT NULL, quantity INT NOT NULL, purchase_date TEXT NOT NULL, expire_date TEXT, image_url TEXT, fridge_id TEXT NOT NULL);";

            connection.Execute(createAppUsersTableSql);
            connection.Execute(createFridgesTableSql);
            connection.Execute(createFridgeItemsTableSql);*/
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(DatabaseFilePath);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(DatabaseFilePath);
        }
    }
}