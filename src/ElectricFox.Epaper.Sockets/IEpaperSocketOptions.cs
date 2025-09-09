namespace ElectricFox.Epaper.Sockets
{
    public interface IEpaperSocketOptions
    {
        string TcpServer { get; }
        int TcpPort { get; }
    }
}
