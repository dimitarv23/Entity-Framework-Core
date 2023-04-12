using Newtonsoft.Json;

namespace Artillery.DataProcessor.ImportDto
{
    [JsonObject("Countries")]
    public class ImportCountryGunDto
    {
        [JsonProperty("Id")]
        public int CountryId { get; set; }
    }
}
