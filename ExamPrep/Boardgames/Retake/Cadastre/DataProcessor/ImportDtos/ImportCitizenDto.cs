using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cadastre.DataProcessor.ImportDtos
{
    
    public class ImportCitizenDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [JsonProperty("LastName")]
        public string LastName { get; set; }
        [Required]
        [JsonProperty("BirthDate")]
        public string BirthDate { get; set; }
        [Required]
        [JsonProperty("MaritalStatus")]
        public string MaritalStatus { get; set; }
        [JsonProperty("Properties")]
        public int[] PropertiesIds { get; set; }
        //        ⦁	Id – integer, Primary Key
        //⦁	FirstName – text with length[2, 30] (required)
        //⦁	LastName – text with length[2, 30] (required)
        //⦁	BirthDate – DateTime(required)
        //⦁	MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed) (required)
        //⦁	PropertiesCitizens - collection of type PropertyCitizen
    }
}
