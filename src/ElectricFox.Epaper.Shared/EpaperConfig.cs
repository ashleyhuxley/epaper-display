using ElectricFox.ConfigManagement;
using Microsoft.Extensions.Logging;

namespace ElectricFox.Epaper.Shared
{
    public class EpaperConfig : ConfigManager
    {
        public EpaperConfig(string baseUrl, string environnment, ILogger<ConfigManager> logger, TimeSpan? refreshInterval = null) 
            : base(baseUrl, logger, environnment, refreshInterval)
        { }

        public string GetDeviceAddress() => Get<string>(Constants.AppName, Constants.DeviceAddress);
        public int GetDevicePort() => Get<int>(Constants.AppName, Constants.DevicePort);
        public string GetHomeAssistantBaseUrl() => Get<string>(Constants.HomeAssistant, Constants.BaseUrl);
        public string GetHomeAssistantApiToken() => Get<string>(Constants.HomeAssistant, Constants.ApiToken);
        public string GetOpenWeatherBaseUrl() => Get<string>(Constants.OpenWeather, Constants.BaseUrl);
        public string GetOpenWeatherApiToken() => Get<string>(Constants.OpenWeather, Constants.ApiToken);
        public double GetLatitude() => Get<double>(Constants.Home, Constants.Latitude);
        public double GetLongitude() => Get<double>(Constants.Home, Constants.Longitude);
        public string GetTimeZone() => Get<string>(Constants.Home, Constants.Timezone);
        public string GetAssetsPath() => Get<string>(Constants.AppName, Constants.AssetsPath);
    }
}
