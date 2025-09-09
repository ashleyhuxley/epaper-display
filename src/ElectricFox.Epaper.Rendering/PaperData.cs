namespace ElectricFox.Epaper.Rendering
{
    public class PaperData
    {
        private const int Width = 272;
        private const int Height = 792;

        public byte[] Red { get; set; } = new byte[Width / 8 * Height];
        public byte[] Black { get; set; } = new byte[Width / 8 * Height];

        public PaperData()
        {
            for (int i = 0; i < Red.Length; i++)
            {
                Red[i] = 255;
                Black[i] = 255;
            }
        }

        public IEnumerable<byte> GetAllData()
        {
            foreach (var b in Black)
            {
                yield return b;
            }
            foreach (var r in Red)
            {
                yield return r;
            }
        }
    }
}
