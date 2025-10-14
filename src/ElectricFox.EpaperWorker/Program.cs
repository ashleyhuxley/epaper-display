using ElectricFox.ConfigManagement;
using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Shared;
using ElectricFox.Epaper.Sockets;
using ElectricFox.HomeAssistant;
using ElectricFox.OpenWeather;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Reflection;
using System.Text;

namespace ElectricFox.EpaperWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            logger.Info($"Starting Epaper Worker");

            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            IConfigurationRoot configRoot = configBuilder.Build();
            var configUrl = configRoot.GetValue<string>("ConfigUrl");
            if (string.IsNullOrEmpty(configUrl))
            {
                logger.Error("ConfigUrl is not set in environment variables or appsettings.json");
                return;
            }

#if DEBUG
            const string env = "dev";
#else
            const string env = "prod";
#endif

            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ConfigManager>>();
                var manager = new EpaperConfig(configUrl, env, logger, TimeSpan.FromMinutes(2));
                manager.ReloadAsync().GetAwaiter().GetResult();
                return manager;
            });

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Workers
            builder.Services.AddHostedService<EpaperWorker>();

            // Services
            builder.Services.AddTransient<IHomeAssistantClient, HomeAssistantClient>();
            builder.Services.AddTransient<IOpenWeatherClient, OpenWeatherClient>();
            builder.Services.AddTransient<IEpaperSocketClient, EpaperSocketClient>();
            builder.Services.AddTransient<EpaperDataService>();

            builder.Services.AddSingleton<HttpClient>();

            var host = builder.Build();
            host.Run();
        }
    }
}