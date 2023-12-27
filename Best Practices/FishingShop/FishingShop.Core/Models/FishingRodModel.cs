using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishingShop.Core.Models
{
    public class FishingRodModel
    {

        [Required(ErrorMessage = "Полето {0} е задължително")]
        [MaxLength(50)]
        [DisplayName("Модел")]
        public string? Model { get; set; }

        [Required(ErrorMessage ="Полето {0} е задължително")]
        [DisplayName("Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително")]
        [DisplayName("Дължина")]
        public decimal Lenght { get; set; }
    }
}
