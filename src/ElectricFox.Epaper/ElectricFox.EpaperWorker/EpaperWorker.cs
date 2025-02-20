using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Sockets;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Options;
using NodaTime;

namespace ElectricFox.EpaperWorker
{
    public class EpaperWorker : BackgroundService
    {
        private readonly RenderingAssets _assets;

        private readonly ILogger<EpaperWorker> _logger;

        private readonly EpaperDataService _epaperDataService;

        private readonly IEpaperSocketClient _epaperSocketClient;

        private readonly OpenWeatherOptions _openWeatherOptions;

        private readonly EpaperRenderingOptions _renderingOptions;

        private readonly DateTimeZone _timeZone;

        public EpaperWorker(
            ILogger<EpaperWorker> logger,
            IOptions<OpenWeatherOptions> openWeatherOptions,
            IOptions<EpaperRenderingOptions> renderingOptions,
            IEpaperSocketClient epaperSocketClient,
            EpaperDataService epaperDataService
        )
        {
            _logger = logger;
            _openWeatherOptions = openWeatherOptions.Value;
            _renderingOptions = renderingOptions.Value;

            _epaperDataService =
                epaperDataService ?? throw new ArgumentNullException(nameof(epaperDataService));
            _epaperSocketClient =
                epaperSocketClient ?? throw new ArgumentNullException(nameof(epaperSocketClient));

            _timeZone = DateTimeZoneProviders.Tzdb[renderingOptions.Value.TimeZone];

            _assets = new RenderingAssets(renderingOptions.Value.AssetsPath);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Gathering data...");

                var state = await _epaperDataService.GetRenderStateAsync(
                    _openWeatherOptions.Latitude,
                    _openWeatherOptions.Longitude,
                    stoppingToken
                );

                using (var renderer = new GraphicsRenderer(_assets, _timeZone))
                {
                    try
                    {
                        _logger.LogInformation("Rendering...");
                        renderer.Render(state);
                        var data = renderer.GetPixelData().GetAllData().ToArray();

                        _logger.LogInformation("Sending to display...");
                        await _epaperSocketClient.SendImage(data);
                        _logger.LogInformation("Display cycle complete.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error rendering ePaper");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(_renderingOptions.UpdateIntervalSeconds), stoppingToken);
            }
        }
    }
}
