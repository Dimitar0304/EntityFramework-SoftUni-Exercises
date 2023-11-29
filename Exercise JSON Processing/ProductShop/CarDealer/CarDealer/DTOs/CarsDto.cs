using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.DTOs
{
    public  class CarsDto
    {
        [JsonProperty("make")]
        public string  Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("traveledDistance")]
        public string  TraveledDistance { get; set; }


        [JsonProperty("partsId")]
        public ICollection<int> PartsIds { get; set; }
    }
}
