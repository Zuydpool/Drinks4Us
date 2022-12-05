using Drinks4Us.Models;
using System.Threading.Tasks;

namespace Drinks4Us.Storage.Dao
{
    public interface IAppUsersDao : IDao<AppUser>
    {
        Task<AppUser?> GetByEmail(string email);

        Task<bool> CheckIfAccountExists(string email);
    }
}