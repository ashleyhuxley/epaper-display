using BdfFontParser;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ElectricFox.Epaper.Rendering
{
    public static class ImageExtensions
    {
        public static void DrawTextBdf(this Image<Rgba32> image, string text, BdfFont font, Point pos, Color color)
        {
            var map = font.GetMapOfString(text);
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

        public static void DrawTextBdf(this Image<Rgba32> image, string text, BdfFont font, Point pos)
        {
            image.DrawTextBdf(text, font, pos, Color.Black);
        }
    }
}
