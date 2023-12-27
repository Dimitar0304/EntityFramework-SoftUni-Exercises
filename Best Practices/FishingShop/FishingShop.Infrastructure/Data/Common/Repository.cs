using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingShop.Infrastructure.Data.Common
{
    public class Repository : IRepository
    {
        private FishingShopDbContext _context;
        public Repository(FishingShopDbContext context)
        {
            _context = context;
        }
        private DbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public  async Task AddAsync<T>(T entity)where T : class
        {
            await GetDbSet<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync<T>(IEnumerable<T> entities)where T : class
        {
            await GetDbSet<T>().AddRangeAsync(entities);
        }

        public IQueryable<T> All<T>()where T : class
        {
           return GetDbSet<T>().AsQueryable();
        }

        public IQueryable<T> AllReadOnly<T>() where T : class
        {
            return GetDbSet<T>().AsQueryable().AsNoTracking();
        }

        public async Task DeleteByIdAsync<T>(int id) where T : class
        {
            T entity = await GetDbSet<T>().FindAsync(id);
            if (entity!=null)
            {

             GetDbSet<T>().Remove(entity);
            }
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            T entity = await GetDbSet<T>().FindAsync(id);
            if (entity != null)
            {
                return entity;
            }
            return default(T);
        }

        public  async Task SaveChangesAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}
