namespace ElectricFox.Epaper.Shared
{
    public sealed class Room
    {
        public required string Name { get; init; }
        public required string Temperature { get; init; }
        public required string Humidity { get; init; }
    }
}
