namespace ElectricFox.OpenWeather
{
    public class OpenWeatherOptions : IOpenWeatherClientOptions
    {
        public double Latitude { get; set; } = 0;
        public double Longitude { get; set; } = 0;
        public string OpenWeatherBaseUrl { get; set; } = "";
        public string OpenWeatherApiToken { get; set; } = "";
    }
}
