using System.Net.Sockets;

namespace ElectricFox.Epaper.Sockets
{
    public class EpaperSocketClient : IEpaperSocketClient
    {
        private readonly IEpaperSocketOptions _options;

        public EpaperSocketClient(IEpaperSocketOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task SendImage(byte[] data)
        {
            using (TcpClient client = new(_options.TcpServer, _options.TcpPort))
            {
                using (NetworkStream stream = client.GetStream())
                {
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
    }
}
