using EventMe.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Configurations
{
    public class EventConfigurations : IEntityTypeConfiguration<Event>
    {
        
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasQueryFilter(e => e.IsActive);
        }
    }
}
