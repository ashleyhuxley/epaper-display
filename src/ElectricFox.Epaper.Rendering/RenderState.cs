using NodaTime;

namespace ElectricFox.Epaper.Rendering
{
    public class RenderState
    {
        public class TemperatureState
        {
            public DateTime DateTime { get; set; }
            public float Temperature { get; set; }
        }

        public class RoomState
        {
            public string RoomName { get; set; } = "";
            public float Temperature { get; set; }
            public float Humidity { get; set; }
        }

        public class WeatherState
        {
            public Instant DateTime { get; set; }
            public Instant Sunrise { get; set; }
            public Instant Sunset { get; set; }
            public float DayTemp { get; set; }
            public float NightTemp { get; set; }
            public float WindSpeed { get; set; }
            public float Humidity { get; set; }
            public float FeelsLike { get; set; }
            public WeatherIcon Icon { get; set; }
        }

        public enum WeatherIcon { ClearSky, FewClouds, ScatteredClouds, BrokenClouds, ShowerRain, Rain, Thunderstorm, Snow, Mist }

        public enum AlarmState { Disarmed, ArmedHome, ArmedAway }

        public Instant DateTime => SystemClock.Instance.GetCurrentInstant();

        public float CurrentTemp { get; set; } = 0;
        public float ThermostatTemp { get; set; } = 0;
        public float CurrentOutsideTemp { get; set; } = 0;
        public bool HeatingOn { get; set; }
        public float PoolTemp { get; set; } = 0;
        public float HotWaterTemp { get; set; } = 0;

        public bool IsNight { get; set; }

        public AlarmState Alarm { get; set; } = AlarmState.Disarmed;
        public bool IsArmed => Alarm != AlarmState.Disarmed;

        public string Bins { get; set; } = "Unknown";

        public float GetTemperatureAt(DateTime dateTime, bool isIndoor = true)
        {
            var temps = isIndoor ? IndoorTempHistory : OutsideTempHistory;

            var temp = temps
                .Where(t => t.DateTime < dateTime)
                .OrderByDescending(t => t.DateTime)
                .FirstOrDefault();

            return temp?.Temperature ?? 0;
        }

        public List<TemperatureState> IndoorTempHistory { get; } = [];
        public List<TemperatureState> OutsideTempHistory { get; } = [];
        public List<RoomState> RoomStates { get; } = [];
        public List<WeatherState> WeatherStates { get; } = [];
    }
}
