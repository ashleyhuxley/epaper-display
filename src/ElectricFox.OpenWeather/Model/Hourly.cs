using System.Text.Json.Serialization;

namespace ElectricFox.OpenWeather.Model
{
    public class Hourly
    {
        [JsonPropertyName("dt")]
        public long? Dt { get; set; }

        [JsonPropertyName("temp")]
        public double? Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double? FeelsLike { get; set; }

        [JsonPropertyName("pressure")]
        public int? Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int? Humidity { get; set; }

        [JsonPropertyName("dew_point")]
        public double? DewPoint { get; set; }

        [JsonPropertyName("uvi")]
        public double? Uvi { get; set; }

        [JsonPropertyName("clouds")]
        public int? Clouds { get; set; }

        [JsonPropertyName("visibility")]
        public int? Visibility { get; set; }

        [JsonPropertyName("wind_speed")]
        public double? WindSpeed { get; set; }

        [JsonPropertyName("wind_deg")]
        public int? WindDeg { get; set; }

        [JsonPropertyName("wind_gust")]
        public double? WindGust { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }

        [JsonPropertyName("pop")]
        public int? Pop { get; set; }
    }
}
