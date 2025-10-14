using ElectricFox.ConfigManagement;
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
            var host = _configManager.GetDeviceAddress();
            var port = _configManager.GetDevicePort();

            using TcpClient client = new(host, port);
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
