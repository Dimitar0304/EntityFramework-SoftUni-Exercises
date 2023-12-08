using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ImportClientDto
    {
        
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        [XmlElement("NumberVat")]
        public string NumberVat { get; set; }
        [XmlArray("Addresses")]
        //[XmlElement("Addresses")]
        public ImportAddressesDto[] ImportAddressesDtos { get; set; } = null!;

    }
//    ⦁	Id – integer, Primary Key
//⦁	Name – text with length[10…25] (required)
//⦁	NumberVat – text with length[10…15] (required)
//⦁	Invoices – collection of type Invoicе
//⦁	Addresses – collection of type Address
//⦁	ProductsClients – collection of type ProductClient

}
