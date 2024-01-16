using EventMe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Core.Contracts
{
    /// <summary>
    /// услуга за събития
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Създаване на събитие
        /// </summary>
        /// <param name="model"></данни на събитието>
        /// <returns></returns>
        public Task Create(EventModel model);
        /// <summary>
        /// редактиране на събитие
        /// </summary>
        /// <param name="model"></данни на събитието>
        /// <returns></returns>
        public Task Edit(EventModel model) ;
        /// <summary>
        /// изтриване на събитие по идентификатор
        /// </summary>
        /// <param name="id"></идентификатор на събитието>
        /// <returns></returns>
        public Task Delete(int id);
        /// <summary>
        /// Връща събитието по Идентификатор
        /// </summary>
        /// <param name="id"></идентификатор на събитие>
        /// <returns></returns>
        public Task<EventModel> GetByIdAsync(int id);
        /// <summary>
        /// Връща всички събития
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<EventModel>> GetAllAsync();
    }
}
