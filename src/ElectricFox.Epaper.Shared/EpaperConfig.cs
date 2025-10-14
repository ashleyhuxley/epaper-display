using ElectricFox.ConfigManagement;
using Microsoft.Extensions.Logging;

namespace ElectricFox.Epaper.Shared
{
    public class EpaperConfig : ConfigManager
    {
        public EpaperConfig(string baseUrl, string environnment, ILogger<ConfigManager> logger, TimeSpan? refreshInterval = null) 
            : base(baseUrl, logger, environnment, refreshInterval)
        { }

        public (string Host, int Port) GetDevice()
        {
            string host = Get<string>(Constants.AppName, Constants.DeviceAddress);
            int port = Get<int>(Constants.AppName, Constants.DevicePort);
            return (host, port);
        }
        
        public string GetHomeAssistantBaseUrl() => Get<string>(Constants.HomeAssistant, Constants.BaseUrl);

        public string GetHomeAssistantApiToken() => Get<string>(Constants.HomeAssistant, Constants.ApiToken);

        public string GetOpenWeatherBaseUrl() => Get<string>(Constants.OpenWeather, Constants.BaseUrl);

        public string GetOpenWeatherApiToken() => Get<string>(Constants.OpenWeather, Constants.ApiToken);

        public (double Latitude, double Longitude) GetCoordinates()
        {
            var lat = Get<double>(Constants.Home, Constants.Latitude);
            var lon = Get<double>(Constants.Home, Constants.Longitude);
            return (lat, lon);
        }

        public string GetTimeZone() => Get<string>(Constants.Home, Constants.Timezone);

        public string GetAssetsPath() => Get<string>(Constants.AppName, Constants.AssetsPath);

        public long GetUpdateInterval() => Get<long>(Constants.AppName, Constants.UpdateInterval);

        public async Task<Sensors?> GetSensorsAsync() => 
            await GetJsonConfig<Sensors?>(Constants.SensorsJson);
    }
}
