using ElectricFox.BdfSharp;
using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Sockets;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Options;
using NodaTime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace ElectricFox.Epaper.Layout
{
    public partial class MainForm : Form
    {
        private EpaperDataService _epaperDataService;

        private IEpaperSocketClient _epaperSocketClient;

        private RenderState? _renderState = new();

        private readonly OpenWeatherOptions _openWeatherOptions;

        private readonly EpaperRenderingOptions _renderingOptions;

        private readonly DateTimeZone _timeZone;

        private byte[]? pictureData = null;

        private BdfFonts? _fonts;
        private Icons? _icons;


        public MainForm(
            IOptions<EpaperRenderingOptions> renderingOptions,
            IOptions<OpenWeatherOptions> openWeatherOptions,
            IEpaperSocketClient epaperSocketClient,
            EpaperDataService epaperDataService
        )
        {
            InitializeComponent();

            _openWeatherOptions = openWeatherOptions.Value;
            _renderingOptions = renderingOptions.Value;

            _epaperDataService =
                epaperDataService ?? throw new ArgumentNullException(nameof(epaperDataService));
            _epaperSocketClient =
                epaperSocketClient ?? throw new ArgumentNullException(nameof(epaperSocketClient));

            _timeZone = DateTimeZoneProviders.Tzdb[renderingOptions.Value.TimeZone];


            sendButton.Enabled = true;
            renderButton.Enabled = true;
        }

        private async void MainFormLoad(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = _renderState;

            _fonts = await LoadFontsAsync(_renderingOptions.AssetsPath);
            _icons = await LoadIconsAsync(_renderingOptions.AssetsPath);
        }

        private void LayoutMouseMove(object sender, MouseEventArgs e)
        {
            this.Text = $"X: {e.X}, Y: {e.Y}";
        }

        private void ShowImageInPictureBox(Image<Rgba32> imageSharpImage)
        {
            using (var ms = new MemoryStream())
            {
                imageSharpImage.SaveAsPng(ms);
                ms.Seek(0, SeekOrigin.Begin);

                Bitmap bitmap = new(ms);

                layout2.Image = bitmap;
            }
        }

        private async void SendButtonClick(object sender, EventArgs e)
        {
            if (pictureData is null)
            {
                return;
            }

            try
            {
                await _epaperSocketClient.SendImage(pictureData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Failed to send data",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void RenderButtonClick(object sender, EventArgs e)
        {
            if (_renderState is null || _fonts is null || _icons is null)
            {
                return;
            }

            this.Text = "Rendering...";
            using (var renderer = new GraphicsRenderer(_fonts, _icons, _timeZone))
            {
                try
                {
                    renderer.Render(_renderState);
                    ShowImageInPictureBox(renderer.GetImage());
                    pictureData = renderer.GetPixelData().GetAllData().ToArray();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Rendering Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }

            this.Text = "Done.";
        }

        private async void GetDataButtonClick(object sender, EventArgs e)
        {
            _renderState = await _epaperDataService.GetRenderStateAsync(
                _openWeatherOptions.Latitude,
                _openWeatherOptions.Longitude,
                CancellationToken.None
            );
            propertyGrid.SelectedObject = _renderState;
        }

        private void LoadImageButtonClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(openFileDialog.FileName))
                    {
                        ShowImageInPictureBox(image);
                        //pictureData = image.GetPixelData().GetAllData().ToArray();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Failed to load image",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        private static async Task<BdfFonts> LoadFontsAsync(string basePath)
        {
            return new BdfFonts
            {
                NcenR18 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/ncenR18.bdf")),
                Spleen8x16 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/spleen-8x16.bdf")),
                Tamzen7x14r = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/tamzen7x14r.bdf")),
                WinCrox5hb = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/win_crox5hb.bdf")),
                TamzenForPowerline10x20b = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/TamzenForPowerline10x20b.bdf")),
                Generic10x20 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/10x20.bdf")),
                Tamzen7x14b = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/Tamzen7x14b.bdf")),
                Generic5x8 = await BdfFont.LoadAsync(Path.Join(basePath, "/Fonts/5x8.bdf")),
            };
        }

        private static async Task<Icons> LoadIconsAsync(string basePath)
        {
            return new Icons
            {
                HouseDay = await Image.LoadAsync(Path.Join(basePath, IconPath.HouseDay)),
                HouseNight = await Image.LoadAsync(Path.Join(basePath, IconPath.HouseNight)),
                Thermometer = await Image.LoadAsync(Path.Join(basePath, IconPath.Thermometer)),
                Thermostat = await Image.LoadAsync(Path.Join(basePath, IconPath.Thermostat)),
                AlarmDisarmed = await Image.LoadAsync(Path.Join(basePath, IconPath.AlarmDisarmed)),
                AlarmArmed = await Image.LoadAsync(Path.Join(basePath, IconPath.AlarmArmed)),
                Trash = await Image.LoadAsync(Path.Join(basePath, IconPath.Trash)),
                WeatherClearSky = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherClearSky)),
                WeatherFewClouds = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherFewClouds)),
                WeatherScatteredClouds = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherScatteredClouds)),
                WeatherBrokenClouds = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherBrokenClouds)),
                WeatherShowerRain = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherShowerRain)),
                WeatherRain = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherRain)),
                WeatherThunderstorm = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherThunderstorm)),
                WeatherSnow = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherSnow)),
                WeatherMist = await Image.LoadAsync(Path.Join(basePath, IconPath.WeatherMist)),
                Sunrise = await Image.LoadAsync(Path.Join(basePath, IconPath.Sunrise)),
                Sunset = await Image.LoadAsync(Path.Join(basePath, IconPath.Sunset)),
                Wind = await Image.LoadAsync(Path.Join(basePath, IconPath.Wind)),
                Humidity = await Image.LoadAsync(Path.Join(basePath, IconPath.Humidity)),
                Pool = await Image.LoadAsync(Path.Join(basePath, IconPath.Pool)),
                PoolHeater = await Image.LoadAsync(Path.Join(basePath, IconPath.PoolHeater))
            };
        }
    }
}
