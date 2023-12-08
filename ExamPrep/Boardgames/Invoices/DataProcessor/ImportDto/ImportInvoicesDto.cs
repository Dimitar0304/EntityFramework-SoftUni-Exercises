using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    [JsonObject("Invoice")]
    public class ImportInvoicesDto
    {
        [Required]

        [Range(1000000000, 1500000000)]
        [JsonProperty("Number")]
        public int Number { get; set; }
        [Required]
        [JsonProperty("IssueDate")]
        public string IssueDate { get; set; }
        [Required]
        [JsonProperty("DueDate")]
        public string DueDate { get; set; }
        [Required]
        [JsonProperty("Amount")]
        public decimal Amount { get; set; }
        [Required]
        [Range(0,2)]
        [JsonProperty("CurrencyType")]
        public int CurrencyType { get; set; }
        [Required]
        [JsonProperty("ClientId")]
        public int ClientId { get; set; }
        //        ⦁	Id – integer, Primary Key
        //⦁	Number – integer in range[1, 000, 000, 000…1, 500, 000, 000] (required)
        //⦁	IssueDate – DateTime(required)
        //⦁	DueDate – DateTime(required)
        //⦁	Amount – decimal (required)
        //⦁	CurrencyType – enumeration of type CurrencyType, with possible values(BGN, EUR, USD) (required)
        //⦁	ClientId – integer, foreign key(required)
        //⦁	Client – Client

    }
}
