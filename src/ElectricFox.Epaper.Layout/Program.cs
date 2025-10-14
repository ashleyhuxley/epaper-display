using ElectricFox.ConfigManagement;
using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Shared;
using ElectricFox.Epaper.Sockets;
using ElectricFox.HomeAssistant;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace ElectricFox.Epaper.Layout
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();

            logger.Info($"Starting Epaper.Layout");

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

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(sp =>
                    {
                        var logger = sp.GetRequiredService<ILogger<ConfigManager>>();
                        var manager = new EpaperConfig(configUrl, env, logger, TimeSpan.FromMinutes(2));
                        manager.ReloadAsync().GetAwaiter().GetResult();
                        return manager;
                    });

                    var configRoot = context.Configuration;

                    services.AddTransient<MainForm>();

                    services.AddTransient<IHomeAssistantClient, HomeAssistantClient>();
                    services.AddTransient<IOpenWeatherClient, OpenWeatherClient>();
                    services.AddTransient<IEpaperSocketClient, EpaperSocketClient>();
                    services.AddTransient<EpaperDataService>();

                    services.AddSingleton<HttpClient>();
                })
                .Build();

            ApplicationConfiguration.Initialize();

            using (var scope = host.Services.CreateScope())
            {
                var mainForm = scope.ServiceProvider.GetRequiredService<MainForm>();
                Application.Run(mainForm);
            }
        }
    }
}