using ElectricFox.ConfigManagement;
using ElectricFox.OpenWeather.Model;
using Microsoft.Extensions.Logging;
using ElectricFox.Epaper.Shared;
using System.Text.Json;

namespace ElectricFox.OpenWeather
{
    public class OpenWeatherClient : IOpenWeatherClient
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;

        private readonly string _apiToken;

        private readonly ILogger<OpenWeatherClient> _logger;

        public OpenWeatherClient(
            ConfigManager configManager,
            HttpClient httpClient,
            ILogger<OpenWeatherClient> logger
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (configManager is null)
            {
                throw new ArgumentNullException(nameof(configManager));
            }

            _baseUrl = configManager.Get<string>(Constants.OpenWeather, Constants.BaseUrl);
            _apiToken = configManager.Get<string>(Constants.OpenWeather, Constants.ApiToken);
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<WeatherData?> GetWeatherDataAsync(
            double latitude,
            double longitude,
            CancellationToken cancellationToken
        )
        {
            var url = new UriBuilder(_baseUrl)
            {
                Query =
                    $"lat={latitude}&lon={longitude}&exclude=minutely,hourly,current&appid={_apiToken}"
            };

            var result = await _httpClient
                .GetStringAsync(url.Uri, cancellationToken)
                .ConfigureAwait(false);

            try
            {
                return JsonSerializer.Deserialize<WeatherData>(result);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize weather data");
                return null;
            }
        }
    }
}
