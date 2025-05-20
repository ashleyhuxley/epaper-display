using NodaTime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static ElectricFox.Epaper.Rendering.RenderState;

namespace ElectricFox.Epaper.Rendering
{
    public class GraphicsRenderer : IDisposable
    {
        private const int width = 272;
        private const int height = 792;

        private RenderState _state;

        private readonly RenderingAssets _assets;

        private readonly Image<Rgba32> _image = new(width, height);

        private bool disposedValue;

        private readonly DateTimeZone _timeZone;

        public GraphicsRenderer(RenderingAssets assets, DateTimeZone timeZone)
        {
            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
            _state = new RenderState();
            _timeZone = timeZone ?? throw new ArgumentNullException(nameof(timeZone));
        }

        public void Render(RenderState state)
        {
            _state = state;

            _image.Mutate(ctx =>
            {
                ctx.BackgroundColor(Color.White);

                RenderTitle(ctx, new Point(5, 5));
                RenderDateAndTime(new Point(5, 45));
                RenderDividerLine(ctx, 70);
                RenderTemperatureChart(ctx, new Point(5, 80));
                RenderDividerLine(ctx, 255);
                RenderTemperatures(ctx, new Point(5, 260));
                RenderDividerLine(ctx, 295);
                RenderAlarmState(ctx, new Point(5, 300));
                RenderDividerLine(ctx, 335);
                RenderRoomStates(new Point(5, 345));
                RenderDividerLine(ctx, 470);
                RenderTrash(ctx, new Point(5, 475));
                RenderDividerLine(ctx, 510);
                RenderTodaysWeather(ctx, new Point(5, 520));
                RenderWeather(ctx, new Point(5, 575));
                RenderDividerLine(ctx, 680);
                RenderPool(ctx, new Point(5, 690));
                RenderDividerLine(ctx, 730);
                RenderDividerLine(ctx, 773);
                RenderFirefly(new Point(5, 778));
            });
        }

        private static void RenderDividerLine(IImageProcessingContext ctx, float y)
        {
            ctx.DrawLine(Color.Black, 1, new PointF(5, y), new PointF(width - 5, y));
        }

        private void RenderPool(IImageProcessingContext ctx, Point pos)
        {
            using (var image = Image.Load(_assets.GetIconPath(Icon.Pool)))
            {
                ctx.DrawImage(image, new Point(pos.X, pos.Y), 1);
            }

            using (var image = Image.Load(_assets.GetIconPath(Icon.PoolHeater)))
            {
                ctx.DrawImage(image, new Point(pos.X + 120, pos.Y), 1);
            }

            _image.DrawTextBdf($"--°C", _assets.WinCrox5hb, new Point(pos.X + 35, pos.Y + 2));
            _image.DrawTextBdf($"--°C", _assets.WinCrox5hb, new Point(pos.X + 155, pos.Y + 2));
        }

        private void RenderFirefly(Point pos)
        {
            _image.DrawTextBdf(
                "Firefly Class Registry No. 404-E-132-4FE274A",
                _assets.Generic5x8,
                pos
            );
        }

        private void RenderTodaysWeather(IImageProcessingContext ctx, Point pos)
        {
            var data = _state.WeatherStates.First();

            var day = data.DateTime.InZone(_timeZone).LocalDateTime.ToString("dddd", null);
            var sunrise = data.Sunrise.InZone(_timeZone).LocalDateTime.ToString("HH:mm", null);
            var sunset = data.Sunset.InZone(_timeZone).LocalDateTime.ToString("HH:mm", null);

            using (var image = Image.Load(_assets.GetIconPath(GetIcon(data.Icon))))
            {
                ctx.DrawImage(image, new Point(pos.X, pos.Y), 1);
            }

            _image.DrawTextBdf(day, _assets.Tamzen7x14b, new Point(pos.X, pos.Y + 30), Color.Red);

            _image.DrawTextBdf(
                $"{data.DayTemp:0.0}°",
                _assets.Generic10x20,
                new Point(pos.X + 70, pos.Y)
            );
            _image.DrawTextBdf(
                $"{data.NightTemp:0.0}°",
                _assets.Spleen8x16,
                new Point(pos.X + 70, pos.Y + 25)
            );

            using (var image = Image.Load(_assets.GetIconPath(Icon.Sunrise)))
            {
                ctx.DrawImage(image, new Point(pos.X + 205, pos.Y), 1);
            }
            using (var image = Image.Load(_assets.GetIconPath(Icon.Sunset)))
            {
                ctx.DrawImage(image, new Point(pos.X + 205, pos.Y + 21), 1);
            }

            using (var image = Image.Load(_assets.GetIconPath(Icon.Humidity)))
            {
                ctx.DrawImage(image, new Point(pos.X + 135, pos.Y), 1);
            }
            using (var image = Image.Load(_assets.GetIconPath(Icon.Wind)))
            {
                ctx.DrawImage(image, new Point(pos.X + 135, pos.Y + 21), 1);
            }

            _image.DrawTextBdf(sunrise, _assets.Tamzen7x14b, new Point(pos.X + 225, pos.Y + 2));
            _image.DrawTextBdf(sunset, _assets.Tamzen7x14b, new Point(pos.X + 225, pos.Y + 23));
            _image.DrawTextBdf(
                $"{data.Humidity}%",
                _assets.Tamzen7x14b,
                new Point(pos.X + 155, pos.Y + 2)
            );
            _image.DrawTextBdf(
                $"{data.WindSpeed:0}mph",
                _assets.Tamzen7x14b,
                new Point(pos.X + 155, pos.Y + 23),
                data.WindSpeed > 10 ? Color.Red : Color.Black
            );
        }

        private void RenderWeather(IImageProcessingContext ctx, Point pos)
        {
            var x = pos.X;
            var y = pos.Y;

            foreach (var weatherData in _state.WeatherStates.Skip(1))
            {
                var day = weatherData
                    .DateTime.InZone(_timeZone)
                    .LocalDateTime.ToString("dddd", null);

                var dayTemp = Math.Round(weatherData.DayTemp, 0);

                using (var image = Image.Load(_assets.GetIconPath(GetIcon(weatherData.Icon))))
                {
                    ctx.DrawImage(image, new Point(x, y), 1);
                }

                _image.DrawTextBdf(day, _assets.Tamzen7x14b, new Point(x, y + 30));
                _image.DrawTextBdf(
                    $"{dayTemp:0}°C",
                    _assets.Generic10x20,
                    new Point(x + 40, y + 5),
                    Color.Black
                );

                x += 90;
                if (x > 192)
                {
                    x = pos.X;
                    y = pos.Y + 52;
                }
            }
        }

        private static string GetIcon(WeatherIcon weatherIcon) =>
            weatherIcon switch
            {
                WeatherIcon.ClearSky => Icon.WeatherClearSky,
                WeatherIcon.FewClouds => Icon.WeatherFewClouds,
                WeatherIcon.ScatteredClouds => Icon.WeatherScatteredClouds,
                WeatherIcon.BrokenClouds => Icon.WeatherBrokenClouds,
                WeatherIcon.ShowerRain => Icon.WeatherShowerRain,
                WeatherIcon.Rain => Icon.WeatherRain,
                WeatherIcon.Thunderstorm => Icon.WeatherThunderstorm,
                WeatherIcon.Snow => Icon.WeatherSnow,
                WeatherIcon.Mist => Icon.WeatherMist,
                _ => Icon.WeatherClearSky,
            };

        private void RenderTrash(IImageProcessingContext ctx, Point pos)
        {
            using (var image = Image.Load(_assets.GetIconPath(Icon.Trash)))
            {
                ctx.DrawImage(image, new Point(pos.X, pos.Y), 1);
            }

            _image.DrawTextBdf(
                _state.Bins,
                _assets.TamzenForPowerline10x20b,
                new Point(pos.X + 31, pos.Y + 8)
            );
        }

        private void RenderRoomStates(Point pos)
        {
            for (int i = 0; i < _state.RoomStates.Count; i++)
            {
                var room = _state.RoomStates[i];

                var y = pos.Y + (i * 20);
                _image.DrawTextBdf(room.RoomName, _assets.Spleen8x16, new Point(pos.X + 5, y));

                var tempColour =
                    room.Temperature < (_state.ThermostatTemp - 1.5) ? Color.Red : Color.Black;
                var humColour = room.Humidity > 60 ? Color.Red : Color.Black;

                _image.DrawTextBdf(
                    $"{room.Temperature:0.0}°C",
                    _assets.Spleen8x16,
                    new Point(pos.X + 130, y),
                    tempColour
                );
                _image.DrawTextBdf(
                    $"{room.Humidity:0}%",
                    _assets.Spleen8x16,
                    new Point(pos.X + 215, y),
                    humColour
                );
            }
        }

        private void RenderAlarmState(IImageProcessingContext ctx, Point pos)
        {
            using (
                var image = Image.Load(
                    _assets.GetIconPath(_state.IsArmed ? Icon.AlarmArmed : Icon.AlarmDisarmed)
                )
            )
            {
                ctx.DrawImage(image, new Point(pos.X, pos.Y), 1);
            }

            string text = _state.Alarm switch
            {
                AlarmState.Disarmed => "DISARMED",
                AlarmState.ArmedHome => "ARMED HOME",
                AlarmState.ArmedAway => "ARMED AWAY",
                _ => "Unknown",
            };

            _image.DrawTextBdf(
                text,
                _assets.TamzenForPowerline10x20b,
                new Point(pos.X + 40, pos.Y + 8),
                _state.IsArmed ? Color.Red : Color.Black
            );
        }

        private void RenderTemperatures(IImageProcessingContext ctx, Point pos)
        {
            var color = _state.HeatingOn ? Color.Red : Color.Black;

            using (var image = Image.Load(_assets.GetIconPath(Icon.Thermometer)))
            {
                ctx.DrawImage(image, new Point(pos.X, pos.Y), 1);
            }

            using (var image = Image.Load(_assets.GetIconPath(Icon.Thermostat)))
            {
                ctx.DrawImage(image, new Point(pos.X + 120, pos.Y), 1);
            }

            _image.DrawTextBdf(
                $"{_state.CurrentTemp:0.0}°C",
                _assets.WinCrox5hb,
                new Point(pos.X + 35, pos.Y + 2),
                color
            );
            _image.DrawTextBdf(
                $"{_state.CurrentOutsideTemp:0.0}°C",
                _assets.WinCrox5hb,
                new Point(pos.X + 155, pos.Y + 2)
            );
        }

        private void RenderTitle(IImageProcessingContext ctx, Point pos)
        {
            using (
                var image = Image.Load(
                    _assets.GetIconPath(_state.IsNight ? Icon.HouseNight : Icon.HouseDay)
                )
            )
            {
                ctx.DrawImage(image, new Point(pos.X, pos.Y), 1);
            }

            _image.DrawTextBdf("Serenity", _assets.NcenR18, new Point(pos.X + 40, pos.Y - 3));
        }

        private void RenderDateAndTime(Point pos)
        {
            _image.DrawTextBdf(_state.Date, _assets.Spleen8x16, new Point(pos.X, pos.Y));
            _image.DrawTextBdf(_state.Time, _assets.Spleen8x16, new Point(pos.X + 200, pos.Y));
        }

        private void RenderTemperatureChart(IImageProcessingContext ctx, PointF pos)
        {
            // X Axis Labels
            var hour = DateTime.Now.AddHours(-5);

            for (int i = 0; i < 6; i++)
            {
                float x = pos.X + (i * 45);
                _image.DrawTextBdf(
                    hour.ToString("HH:mm"),
                    _assets.Tamzen7x14r,
                    new Point((int)x, (int)pos.Y + 155)
                );
                hour = hour.AddHours(1);
            }

            // Y Axis Labels
            var maxInside = _state.IndoorTempHistory.Select(t => t.Temperature).Max();
            var minInside = _state.IndoorTempHistory.Select(t => t.Temperature).Min();
            var maxOutside = _state.OutsideTempHistory.Select(t => t.Temperature).Max();
            var minOutside = _state.OutsideTempHistory.Select(t => t.Temperature).Min();

            var min = Math.Min(minInside, minOutside) - 1;
            var max = Math.Max(maxInside, maxOutside) + 1;
            var tempStep = (max - min) / 8f;

            for (int i = 0; i < 8; i++)
            {
                var temp = min + (tempStep * i);
                var y = (7 - i) * 20;
                _image.DrawTextBdf(
                    $"{Convert.ToInt32(Math.Round(temp, 0)):0}",
                    _assets.Tamzen7x14r,
                    new Point(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y) + y)
                );
            }

            // Axis Lines
            ctx.DrawLine(
                Color.Black,
                2,
                new PointF(pos.X + 20, pos.Y),
                new PointF(pos.X + 20, pos.Y + 150)
            );
            ctx.DrawLine(
                Color.Black,
                2,
                new PointF(pos.X + 20, pos.Y + 150),
                new PointF(pos.X + 250, pos.Y + 150)
            );

            // Temperature Line
            var startTime = DateTime.UtcNow.AddHours(-5);
            var resolutionX = (5 * 60 * 60) / 230;
            float resolutionY = (max - min) / 150f;

            PointF? lastIndoorPoint = null;
            PointF? lastOutdoorPoint = null;

            for (int i = 0; i < 230; i += 2)
            {
                var temp_i = _state.GetTemperatureAt(startTime.AddSeconds(i * resolutionX), true);
                var temp_o = _state.GetTemperatureAt(startTime.AddSeconds(i * resolutionX), false);

                var y_i = (temp_i - min) / resolutionY;
                var y_o = (temp_o - min) / resolutionY;

                var thisPoint_i = new PointF(pos.X + 20 + i, pos.Y + 150 - y_i);
                var thisPoint_o = new PointF(pos.X + 20 + i, pos.Y + 150 - y_o);

                if (lastIndoorPoint.HasValue)
                {
                    ctx.DrawLine(Color.Red, 2, lastIndoorPoint.Value, thisPoint_i);
                }

                if (lastOutdoorPoint.HasValue)
                {
                    ctx.DrawLine(Color.Black, 2, lastOutdoorPoint.Value, thisPoint_o);
                }

                lastIndoorPoint = thisPoint_i;
                lastOutdoorPoint = thisPoint_o;
            }
        }

        public PaperData GetPixelData()
        {
            return _image.GetPixelData();
        }

        public Image<Rgba32> GetImage()
        {
            return _image;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _image.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
