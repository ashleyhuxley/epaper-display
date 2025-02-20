namespace ElectricFox.Epaper.Sockets
{
    public class EpaperSocketOptions : IEpaperSocketOptions
    {
        public string TcpServer { get; init; }
        public int TcpPort { get; init; }
    }
}
