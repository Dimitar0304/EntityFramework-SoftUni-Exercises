using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingShop.Infrastructure.Models
{
    public class FishingRod
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)] 
        public string? Model { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal Lenght { get; set; }
    }
}
