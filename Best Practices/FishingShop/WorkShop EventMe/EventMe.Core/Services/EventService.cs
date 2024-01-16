using EventMe.Core.Contracts;
using EventMe.Core.Models;
using EventMe.Infrastructure.Common;
using EventMe.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Core.Services
{
    public class EventService : IEventService
    {
        /// <summary>
        /// използване на базата данни във бизнес приложението
        /// </summary>
        private IRepository _repository;
        public EventService(IRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Създаване на събитие
        /// </summary>
        /// <param name="model"></данни на събитието>
        /// <returns></returns>
        public async Task Create(EventModel model)
        {
            if (model.Id > 0)
            {
                bool exist = _repository.GetId<Event>(model.Id) != null;
                throw new ArgumentException("Събитието вече съществува");
            }
            Event @event = new Event()
            {
                Id = model.Id,
                Place = model.Place,
                Name = model.Name,
                Start = model.Start,
                End = model.End,

            };
            await _repository.AddAsync(@event);
            await _repository.SaveChanges();
        }
        /// <summary>
        /// редактиране на събитие
        /// </summary>
        /// <param name="model"></данни на събитието>
        /// <returns></returns>
        public async Task Edit(EventModel model)
        {
            Event? ec = await _repository
                .AllAsync<Event>()
                .FirstOrDefaultAsync(e => e.Id == model.Id);
            if (ec == null)
            {
                throw new ArgumentException("Събитието не съществува");
            }
            ec.Place = model.Place;
            ec.Name = model.Name;
            ec.Start = model.Start;
            ec.End = model.End;
            await _repository.SaveChanges();

        }
        /// <summary>
        /// изтриване на събитие по идентификатор
        /// </summary>
        /// <param name="id"></идентификатор на събитието>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            Event? ev =
                await _repository
                .GetId<Event>(id);
            if (ev==null)
            {
                throw new ArgumentException("Събитие с такъв идентификатор не съществува");

            }
             _repository.DeleteAsync<Event>(ev);
            await _repository.SaveChanges();
        }
        /// <summary>
        /// Връща събитието по Идентификатор
        /// </summary>
        /// <param name="id"></идентификатор на събитие>
        /// <returns></returns>
        public async Task<EventModel> GetByIdAsync(int id)
        {
            Event? ev =  await _repository.GetId<Event>(id);
            if (ev==null)
            {
                throw new ArgumentException("събитието не съществува");
            }
            return new EventModel()
            {
                Id = ev.Id,
                Name = ev.Name,
                Place = ev.Place,
                Start = ev.Start,
                End = ev.End,

            };
        }
        /// <summary>
        /// Връща всички събития
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EventModel>> GetAllAsync()
        {
            return await  _repository.AllAsyncReadOnly<Event>()
                .Select(e=>new EventModel()
                {
                    Id =e.Id,
                    Name =e.Name,
                    Place =e.Place,
                    Start =e.Start,
                    End =e.End,
                })
                .ToListAsync();
        }
    }
}
