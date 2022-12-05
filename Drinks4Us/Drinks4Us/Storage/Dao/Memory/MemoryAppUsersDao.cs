using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drinks4Us.Models;

namespace Drinks4Us.Storage.Dao.Memory
{
    public class MemoryAppUsersDao : IAppUsersDao
    {
        public readonly ICollection<AppUser> AppUsers = new List<AppUser>();
        public MemoryAppUsersDao()
        {
            AppUsers.Add(new AppUser
            {
                Id = 1,
                Email = "jan.pillenman@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("JanPillenman123!", App.PasswordHash),
            });
        }

        public Task<AppUser> GetById(int id)
        {
            return Task.Factory.StartNew(() => AppUsers.FirstOrDefault(appUser => appUser.Id == id));
        }

        public Task<AppUser> GetByEmail(string email)
        {
            return Task.FromResult(AppUsers.FirstOrDefault(appUser => appUser.Email == email));
        }

        public Task<bool> CheckIfAccountExists(string email)
        {
            return Task.FromResult(AppUsers.FirstOrDefault(appUser =>
                appUser.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) != null);
        }

        public Task<ICollection<AppUser>> GetAll()
        {
            return Task.Factory.StartNew(() => AppUsers);
        }

        public Task<bool> Delete(int id)
        {
            var appUser = AppUsers.FirstOrDefault(appUser => appUser.Id == id);
            if (appUser == null) return Task.Factory.StartNew(() => false);
            var result = AppUsers.Remove(appUser);
            return Task.Factory.StartNew(() => result);
        }

        public Task<AppUser> Add(AppUser entry)
        {
            AppUsers.Add(entry);
            return Task.Factory.StartNew(() => entry);
        }

        public Task<AppUser> Update(AppUser entry)
        {
            return Task.Factory.StartNew(() => entry);
        }
    }
}