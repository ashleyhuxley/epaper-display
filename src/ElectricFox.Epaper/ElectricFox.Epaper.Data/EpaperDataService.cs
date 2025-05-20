using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ElectricFox.Epaper.Rendering;
using ElectricFox.HomeAssistant;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace ElectricFox.Epaper.Data
{
    public class EpaperDataService
    {
        private readonly IHomeAssistantClient _haClient;

        private readonly IOpenWeatherClient _openWeatherClient;

        private readonly ILogger<EpaperDataService> _logger;

        private const float KelvinOffset = 273.15f;
        private const int TemperatureHistoryHours = 6;
        private const int WeatherDays = 7;

        public EpaperDataService(
            IHomeAssistantClient homeAssistantClient,
            IOpenWeatherClient openWeatherClient,
            ILogger<EpaperDataService> logger
        )
        {
            _haClient =
                homeAssistantClient ?? throw new ArgumentNullException(nameof(homeAssistantClient));
            _openWeatherClient =
                openWeatherClient ?? throw new ArgumentNullException(nameof(openWeatherClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RenderState> GetRenderStateAsync(
            double latitude,
            double longitude,
            CancellationToken stoppingToken
        )
        {
            var state = new RenderState();

            _logger.LogInformation("Getting render state");

            // Day/Night
            _logger.LogDebug("Getting Day/Night state");
            var sun = await _haClient
                .GetSensorState(SensorId.Sun, stoppingToken)
                .ConfigureAwait(false);

            state.IsNight = sun?.State == SensorConstant.BelowHorizon;

            // Climate settings
            _logger.LogDebug("Getting thermostat state");
            var climate = await _haClient
                .GetClimate(SensorId.MainThermostat, stoppingToken)
                .ConfigureAwait(false);

            if (climate is not null)
            {
                state.CurrentTemp = Convert.ToSingle(climate.Attributes.CurrentTemperature);
                state.ThermostatTemp = Convert.ToSingle(climate.Attributes.Temperature);
                state.HeatingOn = climate?.State == SensorConstant.Heat;
            }

            // Temperature History
            _logger.LogDebug("Getting inside temperature history");
            var indoorTemps = GetTempHistory(SensorId.AverageTemperature, stoppingToken);
            await foreach (var indoorTemp in indoorTemps.ConfigureAwait(false))
            {
                state.IndoorTempHistory.Add(indoorTemp);
            }

            _logger.LogDebug("Getting outside temperature history");
            var outdoorTemps = GetTempHistory(SensorId.OutsideTemperature, stoppingToken);
            await foreach (var outdoorTemp in outdoorTemps.ConfigureAwait(false))
            {
                state.OutsideTempHistory.Add(outdoorTemp);
            }


            state.CurrentOutsideTemp = Convert.ToSingle(state.OutsideTempHistory.Last().Temperature);

            // Rooms
            _logger.LogDebug("Getting room states");
            foreach (var room in Room.AllRooms)
            {
                state.RoomStates.Add(
                    await GetRoomState(
                            room.Name,
                            room.TemperatureSensor,
                            room.HumiditySensor,
                            stoppingToken
                        )
                        .ConfigureAwait(false)
                );
            }

            // Bins
            _logger.LogDebug("Getting bin state");
            var bins = await _haClient
                .GetSensorState(SensorId.Trash, stoppingToken)
                .ConfigureAwait(false);
            if (bins?.Attributes is not null && bins.Attributes.Any())
            {
                var dateEntry = bins?.Attributes
                    .FirstOrDefault(prop => DateTime.TryParseExact(
                    prop.Key, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));

                if (dateEntry.HasValue)
                {
                    state.Bins = dateEntry.Value.Value.GetString();
                }
                else
                {
                    state.Bins = SensorConstant.Unknown;
                }
            }

            // Weather
            _logger.LogDebug("Getting weather");
            var weather = await _openWeatherClient
                .GetWeatherDataAsync(latitude, longitude, stoppingToken)
                .ConfigureAwait(false);
            if (weather is not null)
            {
                var dayWeather = weather.Daily.OrderBy(w => w.Dt).Take(WeatherDays);
                foreach (var weatherday in dayWeather)
                {
                    state.WeatherStates.Add(
                        new RenderState.WeatherState
                        {
                            DateTime = Instant.FromUnixTimeSeconds(
                                weatherday.Dt.GetValueOrDefault(0)
                            ),
                            DayTemp = weatherday.Temp.Day.GetValueOrDefault(0) - KelvinOffset,
                            NightTemp = weatherday.Temp.Night.GetValueOrDefault(0) - KelvinOffset,
                            FeelsLike =
                                weatherday.FeelsLike.Day.GetValueOrDefault(0) - KelvinOffset,
                            Sunrise = Instant.FromUnixTimeSeconds(
                                weatherday.Sunrise.GetValueOrDefault(0)
                            ),
                            Sunset = Instant.FromUnixTimeSeconds(
                                weatherday.Sunset.GetValueOrDefault(0)
                            ),
                            WindSpeed =
                                Convert.ToSingle(weatherday.WindSpeed.GetValueOrDefault(0))
                                * 0.62137f,
                            Humidity = Convert.ToSingle(weatherday.Humidity.GetValueOrDefault(0)),
                            Icon = weatherday.Weather.FirstOrDefault()?.Icon switch
                            {
                                "01d" => RenderState.WeatherIcon.ClearSky,
                                "02d" => RenderState.WeatherIcon.FewClouds,
                                "03d" => RenderState.WeatherIcon.ScatteredClouds,
                                "04d" => RenderState.WeatherIcon.BrokenClouds,
                                "09d" => RenderState.WeatherIcon.ShowerRain,
                                "10d" => RenderState.WeatherIcon.Rain,
                                "11d" => RenderState.WeatherIcon.Thunderstorm,
                                "13d" => RenderState.WeatherIcon.Snow,
                                "50d" => RenderState.WeatherIcon.Mist,
                                _ => RenderState.WeatherIcon.ClearSky,
                            },
                        }
                    );
                }
            }

            return state;
        }

        private async IAsyncEnumerable<RenderState.TemperatureState> GetTempHistory(
            string sensorId,
            [EnumeratorCancellation] CancellationToken stoppingToken
        )
        {
            var history = await _haClient
                .GetSensorStateHistory(
                    sensorId,
                    DateTime.UtcNow.AddHours(-TemperatureHistoryHours),
                    stoppingToken
                )
                .ConfigureAwait(false);

            if (history is not null)
            {
                foreach (var historyItem in history)
                {
                    if (float.TryParse(historyItem.State, out var temperature))
                    {
                        yield return new RenderState.TemperatureState
                        {
                            DateTime = historyItem.LastChanged ?? DateTime.MinValue,
                            Temperature = temperature,
                        };
                    }
                }
            }
        }

        private async Task<RenderState.RoomState> GetRoomState(
            string roomName,
            string temperatureId,
            string humidityId,
            CancellationToken stoppingToken
        )
        {
            var roomState = new RenderState.RoomState { RoomName = roomName };

            var temperature = await _haClient
                .GetSensorState(temperatureId, stoppingToken)
                .ConfigureAwait(false);

            if (float.TryParse(temperature?.State, out var temperatureValue))
            {
                roomState.Temperature = temperatureValue;
            }
            else
            {
                roomState.Temperature = 0;
            }

            var humidity = await _haClient
                .GetSensorState(humidityId, stoppingToken)
                .ConfigureAwait(false);

            if (float.TryParse(humidity?.State, out var humidityValue))
            {
                roomState.Humidity = humidityValue;
            }
            else
            {
                roomState.Humidity = 0;
            }

            return roomState;
        }
    }
}
