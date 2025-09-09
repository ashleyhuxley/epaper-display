namespace ElectricFox.Epaper.Sockets
{
    public interface IEpaperSocketClient
    {
        Task SendImage(byte[] data);
    }
}
