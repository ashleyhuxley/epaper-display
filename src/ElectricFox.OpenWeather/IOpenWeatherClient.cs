using ElectricFox.OpenWeather.Model;

namespace ElectricFox.OpenWeather
{
    public interface IOpenWeatherClient
    {
        Task<WeatherData?> GetWeatherDataAsync(double latitude, double longitude, CancellationToken cancellationToken);
    }
}
