using EventMe.Infrastructure.Contracts;
using EventMe.Infrastructure.Data;
using EventMe.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Common
{
    /// <summary>
    /// метод за връзка на данни
    /// </summary>
    public class Repository : IRepository
    {

        /// <summary>
        /// Конструктор за инжектиране на контекста в базата
        /// </summary>
        private readonly EventDbContext context;
        public Repository(EventDbContext _context)
        {
            context = _context;
        }
        /// <summary>
        /// Връща сет от даден тип
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <param name="entity"></елемент>
        /// <returns></returns>
        public DbSet<T> DbSet<T>()where T : class 
        {
            return context.Set<T>();
        }
        /// <summary>
        /// добавя ентити от даден тип в даден сет
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async Task AddAsync<T>(T entity) where T : class
        {
            await  DbSet<T>().AddAsync(entity);
        }
        /// <summary>
        /// връща всички записи от даден тип
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  IQueryable<T> AllAsync<T>() where T : class
        {
            return  DbSet<T>();
        }
        /// <summary>
        /// връща всички записи от даден тип само за четене
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AllAsyncReadOnly<T>() where T : class
        {
            return DbSet<T>().AsNoTracking();
        }
        /// <summary>
        /// връща всички записи дори и изтритите
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AllWithDeleted<T>() where T : class
        {
            return DbSet<T>().IgnoreQueryFilters();
        }
        /// <summary>
        /// връща всички записи дори и изтритите но само за четене
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> AllWithDeletedReadOnly<T>() where T : class
        {
            return DbSet<T>().IgnoreQueryFilters().AsNoTracking();
        }
        /// <summary>
        /// Изтрива запис от определен тип които наследява интерфейса за изтриване
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void DeleteAsync<T>(IDeletable entity) where T : class
        {
            entity.IsActive = false;
            entity.DeletedOn = DateTime.Now;
        }
        /// <summary>
        /// връща даден тип по неговият идентификатор
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetId<T>(int id) where T : class
        {
            return await DbSet<T>().FindAsync(id);
        }
        /// <summary>
        /// Запазва промените в дадената база
        /// </summary>
        /// <returns></returns>

        public async Task<int> SaveChanges() 
        {
            return await context.SaveChangesAsync();
        }
    }
}
