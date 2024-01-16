using EventMe.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Data
{
    /// <summary>
    /// контекст на базата данни
    /// </summary>
    public class EventDbContext:DbContext
    {
        /// <summary>
        /// конструктор на базата данни
        /// </summary>
        /// <param name="options"></param>
        public EventDbContext(DbContextOptions<EventDbContext> options):base(options) 
        {
            
        }

        /// <summary>
        /// он модъл криейт на базата данни
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventDbContext).Assembly);
        }
        
        /// <summary>
        /// сета от събития в базата данни
        /// </summary>
        public  DbSet<Event> Events { get; set; } = null!;
    }
}
