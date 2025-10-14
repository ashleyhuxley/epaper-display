using ElectricFox.Epaper.Shared;
using System.Net.Sockets;

namespace ElectricFox.Epaper.Sockets
{
    public class EpaperSocketClient : IEpaperSocketClient
    {
        private readonly EpaperConfig _configManager;

        public EpaperSocketClient(EpaperConfig configManager)
        {
            _configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
        }

        public async Task SendImage(byte[] data)
        {
            var (Host, Port) = _configManager.GetDevice();

            using TcpClient client = new(Host, Port);
            using NetworkStream stream = client.GetStream();

            int packetSize = 1024;
            int offset = 0;

            while (offset < data.Length)
            {
                // Calculate the size of the current chunk
                int currentPacketSize = Math.Min(packetSize, data.Length - offset);

                // Send the chunk
                await stream.WriteAsync(data.AsMemory(offset, currentPacketSize));
                offset += currentPacketSize;
            }
        }
    }
}
