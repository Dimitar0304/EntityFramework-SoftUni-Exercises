using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingShop.Infrastructure.Data.Common
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(int id) where T:class;

        Task AddAsync<T>(T entity) where T:class;
        Task AddRangeAsync<T>(IEnumerable<T> entities)where T : class;
        Task SaveChangesAsync();
        IQueryable<T> AllReadOnly<T>()where T :class;
        IQueryable<T> All<T>()where T : class;
        Task DeleteByIdAsync<T>(int id) where T :class ;

    }
}
