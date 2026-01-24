using ElectricFox.BdfSharp;
using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Shared;
using ElectricFox.Epaper.Sockets;
using NodaTime;
using SixLabors.ImageSharp;

namespace ElectricFox.EpaperWorker
{
    public class EpaperWorker : BackgroundService
    {
        private readonly EpaperConfig _configManager;

        private readonly ILogger<EpaperWorker> _logger;

        private readonly EpaperDataService _epaperDataService;

        private readonly IEpaperSocketClient _epaperSocketClient;

        private readonly DateTimeZone _timeZone;

        public EpaperWorker(
            EpaperConfig configManager,
            ILogger<EpaperWorker> logger,
            IEpaperSocketClient epaperSocketClient,
            EpaperDataService epaperDataService
        )
        {
            _logger = logger;

            _configManager = 
                configManager ?? throw new ArgumentNullException(nameof(configManager));
            _epaperDataService =
                epaperDataService ?? throw new ArgumentNullException(nameof(epaperDataService));
            _epaperSocketClient =
                epaperSocketClient ?? throw new ArgumentNullException(nameof(epaperSocketClient));

            _timeZone = DateTimeZoneProviders.Tzdb[_configManager.GetTimeZone()];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fonts = await LoadFontsAsync(_configManager.GetAssetsPath());
            var icons = await LoadIconsAsync(_configManager.GetAssetsPath());

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Gathering data...");

                try
                {
                    var (Latitude, Longitude) = _configManager.GetCoordinates();

                    var state = await _epaperDataService.GetRenderStateAsync(
                        Latitude,
                        Longitude,
                        stoppingToken
                    );

                    using var renderer = new GraphicsRenderer(fonts, icons, _timeZone);

                    _logger.LogInformation("Rendering...");
                    renderer.Render(state);
                    var data = renderer.GetPixelData().GetAllData().ToArray();

                    _logger.LogInformation("Sending to display...");
                    await _epaperSocketClient.SendImage(data);
                    _logger.LogInformation("Display cycle complete.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating ePaper Display");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(_configManager.GetUpdateInterval()),
                    stoppingToken
                );
            }
        }

        private static async Task<BdfFonts> LoadFontsAsync(string basePath)
        {
            return new BdfFonts
            {
                NcenR18 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/ncenR18.bdf")),
                Spleen8x16 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/spleen-8x16.bdf")),
                Tamzen7x14r = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/tamzen7x14r.bdf")),
                WinCrox5hb = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/win_crox5hb.bdf")),
                TamzenForPowerline10x20b = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/TamzenForPowerline10x20b.bdf")),
                Generic10x20 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/10x20.bdf")),
                Tamzen7x14b = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/Tamzen7x14b.bdf")),
                Generic5x8 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/5x8.bdf")),
                StreamlineAll = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/streamline_all.bdf"))
            };
        }

        private static async Task<Icons> LoadIconsAsync(string basePath)
        {
            return new Icons 
            { 
                HouseDay = await Image.LoadAsync(Path.Join(basePath, IconPath.HouseDay)),
                HouseNight = await Image.LoadAsync(Path.Join(basePath, IconPath.HouseNight)),
                Thermometer = await Image.LoadAsync(Path.Join(basePath, IconPath.Thermometer)),
                Thermostat = await Image.LoadAsync(Path.Join(basePath, IconPath.Thermostat)),
                AlarmDisarmed = await Image.LoadAsync(Path.Join(basePath, IconPath.AlarmDisarmed)),
                AlarmArmed = await Image.LoadAsync(Path.Join(basePath, IconPath.AlarmArmed)),
                Trash = await Image.LoadAsync(Path.Join(basePath, IconPath.Trash)),
                WeatherClearSky = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherClearSky)),
                WeatherFewClouds = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherFewClouds)),
                WeatherScatteredClouds = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherScatteredClouds)),
                WeatherBrokenClouds = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherBrokenClouds)),
                WeatherShowerRain = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherShowerRain)),
                WeatherRain = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherRain)),
                WeatherThunderstorm = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherThunderstorm)),
                WeatherSnow = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherSnow)),
                WeatherMist = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherMist)),
                Sunrise = await Image.LoadAsync(Path.Join(basePath, IconPath.Sunrise)),
                Sunset = await Image.LoadAsync(Path.Join(basePath, IconPath.Sunset)),
                Wind = await Image.LoadAsync(Path.Join(basePath, IconPath.Wind)),
                Humidity = await Image.LoadAsync(Path.Join(basePath, IconPath.Humidity)),
                Pool = await Image.LoadAsync(Path.Join(basePath, IconPath.Pool)),
                PoolHeater = await Image.LoadAsync(Path.Join(basePath, IconPath.PoolHeater))
            };
        }
    }
}
