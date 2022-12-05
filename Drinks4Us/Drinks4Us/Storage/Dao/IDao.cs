using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drinks4Us.Storage.Dao
{
    public interface IDao<T>
    {
        Task<T?> GetById(int id);

        Task<ICollection<T>> GetAll();

        Task<bool> Delete(int id);

        Task<T> Add(T entry);

        Task<T> Update(T entry);
    }
}