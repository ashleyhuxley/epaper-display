using SixLabors.ImageSharp;

namespace ElectricFox.Epaper.Rendering
{
    public class Icons
    {
        public required Image HouseDay { get; init; }
        public required Image HouseNight { get; init; }
        public required Image Thermometer { get; init; }
        public required Image Thermostat { get; init; }
        public required Image AlarmDisarmed { get; init; }
        public required Image AlarmArmed { get; init; }
        public required Image Trash { get; init; }
        public required Image WeatherClearSky { get; init; }
        public required Image WeatherFewClouds { get; init; }
        public required Image WeatherScatteredClouds { get; init; }
        public required Image WeatherBrokenClouds { get; init; }
        public required Image WeatherShowerRain { get; init; }
        public required Image WeatherRain { get; init; }    
        public required Image WeatherThunderstorm { get; init; }
        public required Image WeatherSnow { get; init; }
        public required Image WeatherMist { get; init; }
        public required Image Sunrise { get; init; }
        public required Image Sunset { get; init; }
        public required Image Wind { get; init; }
        public required Image Humidity { get; init; }
        public required Image Pool { get; init; }
        public required Image PoolHeater { get; init; }
    }

    public class IconPath
    {
        public const string HouseDay = "/Icons/house-sun.png";
        public const string HouseNight = "/Icons/house-moon.png";
        public const string Thermometer = "/Icons/thermometer.png";
        public const string Thermostat = "/Icons/thermostat.png";
        public const string AlarmDisarmed = "/Icons/siren-disarmed.png";
        public const string AlarmArmed = "/Icons/siren-armed.png";
        public const string Trash = "/Icons/trash.png";
        public const string WeatherClearSky = "/Icons/weather-clear.png";
        public const string WeatherFewClouds = "/Icons/weather-fewclouds.png";
        public const string WeatherScatteredClouds = "/Icons/weather-scatteredclouds.png";
        public const string WeatherBrokenClouds = "/Icons/weather-brokenclouds.png";
        public const string WeatherShowerRain = "/Icons/weather-showerrain.png";
        public const string WeatherRain = "/Icons/weather-rain.png";
        public const string WeatherThunderstorm = "/Icons/weather-thunder.png";
        public const string WeatherSnow = "/Icons/weather-snow.png";
        public const string WeatherMist = "/Icons/weather-mist.png";
        public const string Sunrise = "/Icons/sunrise.png";
        public const string Sunset = "/Icons/sunset.png";
        public const string Wind = "/Icons/wind.png";
        public const string Humidity = "/Icons/humidity.png";
        public const string Pool = "/Icons/pool.png";
        public const string PoolHeater = "/Icons/poolheater.png";
    }
}
