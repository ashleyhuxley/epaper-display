using System.Text.Json.Serialization;

namespace ElectricFox.OpenWeather.Model
{
    public class WeatherData
    {
        [JsonPropertyName("lat")]
        public double? Lat { get; set; }

        [JsonPropertyName("lon")]
        public double? Lon { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("timezone_offset")]
        public decimal? TimezoneOffset { get; set; }

        [JsonPropertyName("daily")]
        public List<Daily> Daily { get; set; }
    }
}
