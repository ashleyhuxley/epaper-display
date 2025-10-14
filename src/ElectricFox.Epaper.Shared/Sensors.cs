namespace ElectricFox.Epaper.Shared
{
    public class Sensors
    {
        public required string Sun { get; init; }
        public required string MainThermostat { get; init; }
        public required string AverageTemperature { get; init; }
        public required string OutsideTemperature { get; init; }
        public required string Trash { get; init; }
        public required string PoolTemp { get; init; }
        public required string HotWaterTemp { get; init; }
        public required List<Room> Rooms { get; init; }
    }
}
