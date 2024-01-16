using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Contracts
{
    public interface IDeletable
    {
        /// <summary>
        /// Активно ли е събитието
        /// summary
        public bool IsActive { get; set; }
        /// <summary>
        /// Дата на изтриването на дадено ентити
        /// </summary>
        public DateTime DeletedOn { get; set; }
    }
}
