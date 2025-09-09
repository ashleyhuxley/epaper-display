using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Sockets;
using ElectricFox.HomeAssistant;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ElectricFox.Epaper.Layout
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddUserSecrets(Assembly.GetExecutingAssembly());
                }).ConfigureServices((context, services) =>
                {
                    var configRoot = context.Configuration;

                    services.Configure<OpenWeatherOptions>(configRoot.GetSection("OpenWeather"));
                    services.Configure<EpaperSocketOptions>(configRoot.GetSection("EpaperSocket"));
                    services.Configure<EpaperRenderingOptions>(configRoot.GetSection("EpaperRendering"));
                    services.Configure<HomeAssistantOptions>(configRoot.GetSection("HomeAssistant"));

                    services.AddTransient<MainForm>();

                    services.AddTransient<IHomeAssistantClient, HomeAssistantClient>();
                    services.AddTransient<IOpenWeatherClient, OpenWeatherClient>();
                    services.AddTransient<IEpaperSocketClient, EpaperSocketClient>();
                    services.AddTransient<EpaperDataService>();

                    services.AddTransient<IHomeAssistantClientOptions>(s => s.GetRequiredService<IOptions<HomeAssistantOptions>>().Value);
                    services.AddTransient<IOpenWeatherClientOptions>(s => s.GetRequiredService<IOptions<OpenWeatherOptions>>().Value);
                    services.AddTransient<IEpaperSocketOptions>(s => s.GetRequiredService<IOptions<EpaperSocketOptions>>().Value);

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