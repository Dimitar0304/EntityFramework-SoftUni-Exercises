using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDto
    {
        [Required]
        [MinLength(9)]
        [MaxLength(30)]
        [JsonProperty("Name")]
        public string  Name { get; set; }
        [Required]
        [Range(5.00, 1000.00)]
        [JsonProperty("Price")]
        public decimal Price { get; set; }
        [Required]
        [Range(0, 4)]
        [JsonProperty("CategoryType")]
        public int CategoryType { get; set; }
        [JsonProperty("Clients")]
        public int[] ClientIds { get; set; }
        //        ⦁	Id – integer, Primary Key
        //⦁	Name – text with length[9…30] (required)
        //⦁	Price – decimal in range[5.00…1000.00] (required)
        //⦁	CategoryType – enumeration of type CategoryType, with possible values(ADR, Filters, Lights, Others, Tyres) (required)
        //⦁	ProductsClients – collection of type ProductClient

    }
}
