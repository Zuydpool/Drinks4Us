using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.SQLite
{
    public class SQLiteAppUsersDao : IAppUsersDao
    {
        private readonly SQLiteStorage storage;

        public SQLiteAppUsersDao(SQLiteStorage storage)
        {
            this.storage = storage;
        }

        public async Task<AppUser?> GetById(int id)
        {
            return await storage.GetAsyncConnection().Table<AppUser>().FirstOrDefaultAsync(appUser => appUser.Id == id);
        }

        public async Task<ICollection<AppUser>> GetAll()
        {
            var result = await storage.GetAsyncConnection().Table<AppUser>().ToListAsync() ?? new List<AppUser>();

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            return await storage.GetAsyncConnection().DeleteAsync<AppUser>(id).ContinueWith((t) => t.IsCompleted);
        }

        public async Task<AppUser> Add(AppUser entry)
        {
            return await storage.GetAsyncConnection().InsertAsync(entry).ContinueWith((_) => entry);
        }

        public async Task<AppUser> Update(AppUser entry)
        {
            return await storage.GetAsyncConnection().UpdateAsync(entry).ContinueWith((_) => entry);
        }

        public async Task<AppUser> GetByEmail(string email)
        {
            return await storage.GetAsyncConnection().Table<AppUser>().FirstOrDefaultAsync(appUser => appUser.Email == email);
        }

        public Task<bool> CheckIfAccountExists(string email)
        {
            return storage.GetAsyncConnection().Table<AppUser>().FirstOrDefaultAsync(appUser => appUser.Email == email)
                .ContinueWith((t) => t.Result != null);
        }
    }
}