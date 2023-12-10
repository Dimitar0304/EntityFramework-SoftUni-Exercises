using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        [RegularExpression(@"\(\d{3}\) \d{3}-\d{4}")]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Required]
        [XmlAttribute("non-stop")]
        [MinLength(4)]
        [MaxLength(5)]
        public string IsNonStop { get; set; }
        [XmlArray("Medicines")]
        public ImportMedicineDto[] Medicines { get; set; }
    }
//    ⦁	Id – integer, Primary Key
//⦁	Name – text with length[2, 50] (required)
//⦁	PhoneNumber – text with length 14. (required)
//⦁	All phone numbers must have the following structure: three digits enclosed in parentheses, followed by a space, three more digits, a hyphen, and four final digits: 
//⦁	Example -> (123) 456-7890 
//⦁	IsNonStop – bool (required)
//⦁	Medicines - collection of type Medicine

}
