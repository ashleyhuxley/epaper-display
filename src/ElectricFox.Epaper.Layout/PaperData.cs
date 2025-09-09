namespace ElectricFox.Epaper.Layout
{
    public class PaperData
    {
        public byte[] Red { get; set; } = new byte[(272 / 8) * 792];
        public byte[] Black { get; set; } = new byte[(272 / 8) * 792];

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
