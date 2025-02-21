using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Sockets;
using ElectricFox.HomeAssistant;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Options;
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
            var configBuilder = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .AddEnvironmentVariables()
                            .AddUserSecrets(Assembly.GetExecutingAssembly());

            LogManager.Setup().LoadConfigurationFromAppSettings();

            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug("Initialising Epaper Display Worker");

            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddLogging(
                c => {
                    c.ClearProviders();
                    c.AddNLog();
                }
            );

            IConfigurationRoot configRoot = configBuilder.Build();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Config
            builder.Services.Configure<OpenWeatherOptions>(configRoot.GetSection("OpenWeather"));
            builder.Services.Configure<EpaperSocketOptions>(configRoot.GetSection("EpaperSocket"));
            builder.Services.Configure<EpaperRenderingOptions>(configRoot.GetSection("EpaperRendering"));
            builder.Services.Configure<HomeAssistantOptions>(configRoot.GetSection("HomeAssistant"));

            builder.Services.AddTransient<IHomeAssistantClientOptions>(s => s.GetRequiredService<IOptions<HomeAssistantOptions>>().Value);
            builder.Services.AddTransient<IOpenWeatherClientOptions>(s => s.GetRequiredService<IOptions<OpenWeatherOptions>>().Value);
            builder.Services.AddTransient<IEpaperSocketOptions>(s => s.GetRequiredService<IOptions<EpaperSocketOptions>>().Value);

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