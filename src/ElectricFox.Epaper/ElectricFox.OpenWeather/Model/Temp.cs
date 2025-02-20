using System.Text.Json.Serialization;

namespace ElectricFox.OpenWeather.Model
{
    public class Temp
    {
        [JsonPropertyName("day")]
        public float? Day { get; set; }

        [JsonPropertyName("min")]
        public float? Min { get; set; }

        [JsonPropertyName("max")]
        public float? Max { get; set; }

        [JsonPropertyName("night")]
        public float? Night { get; set; }

        [JsonPropertyName("eve")]
        public float? Eve { get; set; }

        [JsonPropertyName("morn")]
        public float? Morn { get; set; }
    }
}
