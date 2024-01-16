using EventMe.Core.Contracts;
using EventMe.Core.Services;
using EventMe.Infrastructure.Common;
using EventMe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace EventMe.Extentions
{
    public static class EventServiceCollectionExtention
    {
        public static IServiceCollection AddAplicationService(this IServiceCollection service)
        {
            service.AddScoped<IEventService, EventService>();
            return service;
        }
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<IRepository,Repository>();
            
            service.AddDbContext<EventDbContext>(options =>
            options.UseSqlServer
            ("Server=DESKTOP-S4CE7G8\\SQLEXPRESS;Database=EventMe;Integrated Security=True;Encrypt"));
            return service;
        }
    }
}
