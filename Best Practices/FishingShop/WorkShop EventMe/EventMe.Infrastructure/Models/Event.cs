using EventMe.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Infrastructure.Models
{
    public class Event : IDeletable
    {
        /// <summary>
        /// Идентификатор на събитието
        /// </summary>
        [Key]
        [Comment("Идентификатор на събитието")]
        public int Id { get; set; }
        /// <summary>
        /// Име на събитието
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Comment("Име на събитието")]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Начало на събитието
        /// </summary>
        [Required]
        [Comment("Начало на събитието")]
        public DateTime Start { get; set; }
        /// <summary>
        /// Край на събитието
        /// </summary>
        [Required]
        [Comment("Край на събитието")]
        public DateTime End { get; set; }
        /// <summary>
        /// Място на събитието
        /// </summary>
        [Required]
        [Comment("Място на събитието")]
        public string Place { get; set; } = null!;
        /// <summary>
        /// Активно ли е събитието
        /// </summary>
        [Required]
        [Comment("Активно ли е събитието")]
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Дата на изтриване на събитието
        /// </summary>
        [Comment("Дата на изтриване на събитието")]
        public DateTime DeletedOn { get ; set ; }
    }
}
