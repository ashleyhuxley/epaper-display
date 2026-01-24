using ElectricFox.BdfSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ElectricFox.Epaper.Rendering
{
    public static class ImageExtensions
    {
        public static void DrawTextBdf(
            this Image<Rgba32> image,
            string text,
            BdfFont font,
            Point pos
        )
        {
            image.DrawTextBdf(text, font, pos, Color.Black);
        }

        public static void DrawTextBdf(
            this Image<Rgba32> image,
            IEnumerable<int> glyphs,
            BdfFont font,
            Point pos,
            Color color
        )
        {
            var map = font.RenderBitmap(glyphs, GlyphLookupOption.EncodingStrict);
            DrawBdfMap(image, map, pos, color);
        }

        public static void DrawTextBdf(
            this Image<Rgba32> image,
            string text,
            BdfFont font,
            Point pos,
            Color color
        )
        {
            var map = font.RenderBitmap(text);
            DrawBdfMap(image, map, pos, color);
        }

        private static void DrawBdfMap(Image<Rgba32> image, bool[,] map, Point pos, Color color)
        {
            var fwidth = map.GetLength(0);
            var fheight = map.GetLength(1);

            for (int y = 0; y < fheight; y++)
            {
                for (int x = 0; x < fwidth; x++)
                {
                    var pixelColor = map[x, y] ? color : Color.White;
                    var absX = pos.X + x;
                    var absY = pos.Y + y;
                    if (absX >= 0 && absY >= 0 && absX < image.Width && absY < image.Height)
                    {
                        image[absX, absY] = pixelColor;
                    }
                }
            }
        }

        public static PaperData GetPixelData(this Image<Rgba32> image)
        {
            var data = new PaperData();

            image.ProcessPixelRows(pixelAccessor =>
            {
                for (int y = 0; y < pixelAccessor.Height; y++)
                {
                    Span<Rgba32> row = pixelAccessor.GetRowSpan(y);

                    for (int x = 0; x < row.Length; x++)
                    {
                        var color = row[x];

                        var x1 = y;
                        var y1 = row.Length - x - 1;

                        var index = (y1 * (792 / 8)) + (x1 / 8);
                        var offset = x1 % 8;

                        const int threshold = 253;

                        if (color.R > threshold && color.G < threshold && color.B < threshold)
                        {
                            data.Red[index] &= (byte)~(0x80 >> offset);
                        }
                        else if (color.R < threshold && color.G < threshold && color.B < threshold)
                        {
                            data.Black[index] &= (byte)~(0x80 >> offset);
                        }
                    }
                }
            });

            return data;
        }
    }
}
