using BdfFontParser;

namespace ElectricFox.Epaper.Rendering
{
    public class RenderingAssets
    {
        private readonly string _basePath;

        public RenderingAssets(string basePath)
        {
            _basePath = basePath;

            NcenR18 = new BdfFont(Path.Join(basePath, "/Fonts/ncenR18.bdf"));
            Spleen8x16 = new BdfFont(Path.Join(basePath, "/Fonts/spleen-8x16.bdf"));
            Tamzen7x14r = new BdfFont(Path.Join(basePath, "/Fonts/tamzen7x14r.bdf"));
            WinCrox5hb = new BdfFont(Path.Join(basePath, "/Fonts/win_crox5hb.bdf"));
            TamzenForPowerline10x20b = new BdfFont(Path.Join(basePath, "/Fonts/TamzenForPowerline10x20b.bdf"));
            Generic10x20 = new BdfFont(Path.Join(basePath, "/Fonts/10x20.bdf"));
            Tamzen7x14b = new BdfFont(Path.Join(basePath, "/Fonts/Tamzen7x14b.bdf"));
            Generic5x8 = new BdfFont(Path.Join(basePath, "/Fonts/5x8.bdf"));
        }

        public string GetIconPath(string icon)
        {
            return Path.Join(_basePath, icon);
        }

        public BdfFont NcenR18 { get; private set; }
        public BdfFont Spleen8x16 {  get; private set; }
        public BdfFont Tamzen7x14r { get; private set; }
        public BdfFont WinCrox5hb { get; private set; }
        public BdfFont TamzenForPowerline10x20b { get; private set; }
        public BdfFont Generic10x20 { get; private set; }
        public BdfFont Tamzen7x14b { get; private set; }
        public BdfFont Generic5x8 { get; private set; }

    }

    public class Icon
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
