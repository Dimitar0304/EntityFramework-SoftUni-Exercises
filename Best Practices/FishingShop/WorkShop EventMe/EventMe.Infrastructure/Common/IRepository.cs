using EventMe.Infrastructure.Contracts;
using EventMe.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Common
{
    public interface IRepository
    {
        /// <summary>
        /// Добавяне на ентити в базата
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <param name="entity"></елемента>
        /// <returns></returns>
        public Task AddAsync<T>(T entity)where T : class;
        /// <summary>
        /// Връща проекция на даден тип от базата
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <returns></returns>
        public IQueryable<T> AllAsync<T>()where T:class;
        /// <summary>
        /// Връща прекция на даден елемент от базата само за четене
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <returns></returns>
        public IQueryable<T> AllAsyncReadOnly<T>()where T:class;
        /// <summary>
        /// Изтрива запис на даден елемент от базата
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <param name="entity"></елемента>
        public void DeleteAsync<T>(IDeletable entity) where T : class;
        /// <summary>
        /// Връща проекция на всички включително и изтритите записи за даден тип
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <returns></returns>
        public IQueryable<T>AllWithDeleted<T>() where T : class;
        /// <summary>
        /// Връща проекция на всички включително и изтритите записи за даден тип само за четене
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <returns></returns>
        public IQueryable<T>AllWithDeletedReadOnly<T>() where T : class;
        /// <summary>
        /// взима даден тип от базата по идентификатора му
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <param name="id"></Идентификатор на елемента>
        /// <returns></returns>
        public Task<T?>GetId<T>(int id)where T : class;
        /// <summary>
        /// Запазва промени в базата
        /// </summary>
        /// <typeparam name="T"></тип на елемента>
        /// <param name="entity"></елемента>
        /// <returns></returns>
        public Task<int> SaveChanges();
        


    }
}
