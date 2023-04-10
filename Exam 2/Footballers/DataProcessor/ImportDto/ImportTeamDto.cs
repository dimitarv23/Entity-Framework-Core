using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MaxLength(40)]
        [RegularExpression("^[a-zA-Z0-9 .-]{3,}$")]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; }

        [Required]
        [JsonProperty("Trophies")]
        public int Trophies { get; set; }

        [JsonProperty("Footballers")]
        public List<int> FootballerIds { get; set; } = new List<int>();
    }
}
