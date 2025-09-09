using System.Text.Json.Serialization;

namespace ElectricFox.OpenWeather.Model
{
    public class Daily
    {
        [JsonPropertyName("dt")]
        public long? Dt { get; set; }

        [JsonPropertyName("sunrise")]
        public long? Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long? Sunset { get; set; }

        [JsonPropertyName("moonrise")]
        public long? Moonrise { get; set; }

        [JsonPropertyName("moonset")]
        public long? Moonset { get; set; }

        [JsonPropertyName("moon_phase")]
        public decimal? MoonPhase { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("temp")]
        public Temp Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public FeelsLike FeelsLike { get; set; }

        [JsonPropertyName("pressure")]
        public double? Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public double? Humidity { get; set; }

        [JsonPropertyName("dew_point")]
        public double? DewPoint { get; set; }

        [JsonPropertyName("wind_speed")]
        public double? WindSpeed { get; set; }

        [JsonPropertyName("wind_deg")]
        public double? WindDeg { get; set; }

        [JsonPropertyName("wind_gust")]
        public double? WindGust { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }

        [JsonPropertyName("clouds")]
        public int? Clouds { get; set; }

        [JsonPropertyName("pop")]
        public double? Pop { get; set; }

        [JsonPropertyName("uvi")]
        public double? Uvi { get; set; }

        [JsonPropertyName("rain")]
        public double? Rain { get; set; }
    }
}
