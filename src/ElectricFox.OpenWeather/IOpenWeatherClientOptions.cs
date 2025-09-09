namespace ElectricFox.OpenWeather
{
    public interface IOpenWeatherClientOptions
    {
        string OpenWeatherBaseUrl { get; set; }
        string OpenWeatherApiToken { get; set; }
    }
}
