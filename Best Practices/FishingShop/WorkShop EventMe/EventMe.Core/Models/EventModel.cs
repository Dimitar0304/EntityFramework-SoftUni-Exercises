using EventMe.Core.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMe.Core.Models
{
    public class EventModel
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Име на събитието
        /// </summary>
        [Required(ErrorMessage = UserMessageConstants.Required)]

        [Display(Name="Име на събитието")]
        [StringLength(50,MinimumLength =10,ErrorMessage =UserMessageConstants.StringLenght)]
        public string Name { get; set; } = null!;
        /// <summary>
        /// Начало на събитието
        /// </summary>
        [Required(ErrorMessage = UserMessageConstants.Required)]
        [Display(Name="Начало на събитието")]
        public DateTime Start { get; set; }
        /// <summary>
        /// Край на събитието
        /// </summary>
        [Required(ErrorMessage = UserMessageConstants.Required)]
        [Display(Name="Край на събитието")]
        public DateTime End { get; set; }
        /// <summary>
        /// Място на събитието
        /// </summary>
        [Required(ErrorMessage =UserMessageConstants.Required)]
        [Display(Name = "Място на събитието")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = UserMessageConstants.StringLenght)]
        public string Place { get; set; } = null!;
       
    }
}
