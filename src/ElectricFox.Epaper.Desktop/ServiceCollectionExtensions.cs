using System;
using System.Net.Http;
using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Desktop.ViewModels;
using ElectricFox.Epaper.Shared;
using ElectricFox.Epaper.Sockets;
using ElectricFox.HomeAssistant;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ElectricFox.Epaper.Desktop;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {                  
        collection.AddTransient<IHomeAssistantClient, HomeAssistantClient>();
        collection.AddTransient<IOpenWeatherClient, OpenWeatherClient>();
        collection.AddTransient<IEpaperSocketClient, EpaperSocketClient>();
        collection.AddTransient<EpaperDataService>();
        collection.AddSingleton<HttpClient>();
        collection.AddTransient(GetEpaperConfig);
        collection.AddTransient<MainWindowViewModel>();
    }

    private static EpaperConfig GetEpaperConfig(IServiceProvider arg)
    {
        const string baseUrl = "";
        var logger = arg.GetRequiredService<ILogger<EpaperConfig>>();
        return new EpaperConfig(baseUrl, "dev", logger, TimeSpan.FromMinutes(10));
    }
}