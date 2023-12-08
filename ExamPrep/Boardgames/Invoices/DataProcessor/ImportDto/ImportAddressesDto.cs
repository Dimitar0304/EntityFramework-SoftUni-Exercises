using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressesDto
    {
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        [XmlElement("StreetName")]
        public string StreetName { get; set; }
        [Required]
        [XmlElement("StreetNumber")]
        public int StreetNumber { get; set; }
        [Required]
        [XmlElement("PostCode")]
        public string PostCode { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        [XmlElement("City")]
        public string City { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        [XmlElement("Country")]
        public string Country { get; set; }

    }
//    ⦁	Id – integer, Primary Key
//⦁	StreetName – text with length[10…20] (required)
//⦁	StreetNumber – integer(required)
//⦁	PostCode – text(required)
//⦁	City – text with length[5…15] (required)
//⦁	Country – text with length[5…15] (required)
//⦁	ClientId – integer, foreign key(required)
//⦁	Client – Client

}