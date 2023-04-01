using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.DTOs.Import
{
    public class CarDTO
    {
        [JsonProperty("make")]
        public string Make { get; set; } = null!;

        [JsonProperty("model")]
        public string Model { get; set; } = null!;

        [JsonProperty("traveledDistance")]
        public long TravelledDistance { get; set; }

        [JsonProperty("partsId")]
        [NotMapped]
        public List<int> PartsId { get; set; } = null!;
    }
}
