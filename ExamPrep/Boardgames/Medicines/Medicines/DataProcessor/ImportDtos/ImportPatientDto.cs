using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        [JsonProperty("FullName")]
        public string FullName { get; set; }
        [Required]
        [JsonProperty("AgeGroup")]
      
        
        public string AgeGroup { get; set; }
        [Required]
        [JsonProperty("Gender")]
        public string Gender { get; set; }
        [Required]
        [JsonProperty("Medicines")]
        public int[] Medicines { get; set; }
    }
//    ⦁	Id – integer, Primary Key
//⦁	FullName – text with length[5, 100] (required)
//⦁	AgeGroup – AgeGroup enum (Child = 0, Adult, Senior) (required)
//⦁	Gender – Gender enum (Male = 0, Female) (required)
//⦁	PatientsMedicines - collection of type PatientMedicine

}
