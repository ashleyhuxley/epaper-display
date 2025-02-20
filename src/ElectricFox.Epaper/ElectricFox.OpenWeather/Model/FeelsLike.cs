using System.Text.Json.Serialization;

namespace ElectricFox.OpenWeather.Model
{
    public class FeelsLike
    {
        [JsonPropertyName("day")]
        public float? Day { get; set; }

        [JsonPropertyName("night")]
        public float? Night { get; set; }

        [JsonPropertyName("eve")]
        public float? Eve { get; set; }

        [JsonPropertyName("morn")]
        public float? Morn { get; set; }
    }
}
