using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [Required]
        [MaxLength(7)]
        [MinLength(2)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(7)]
        [MinLength(2)]
        [XmlElement("LastName")]
        public string LastName { get; set; }
        [XmlArray("Boardgames")]
        public ImportCreatorBoardgameDto[] ImportCreatorBoardgameDtos { get; set; } = null!;
    }
//    ⦁	Id – integer, Primary Key
//⦁	FirstName – text with length[2, 7] (required) 
//⦁	LastName – text with length[2, 7] (required)
//⦁	Boardgames – collection of type Boardgame

}
