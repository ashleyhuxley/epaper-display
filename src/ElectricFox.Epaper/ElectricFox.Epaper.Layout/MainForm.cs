using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Sockets;
using ElectricFox.OpenWeather;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.TimeZones;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ElectricFox.Epaper.Layout
{
    public partial class MainForm : Form
    {
        private EpaperDataService _epaperDataService;

        private IEpaperSocketClient _epaperSocketClient;

        private RenderingAssets _assets;

        private RenderState? _renderState = new();

        private readonly OpenWeatherOptions _openWeatherOptions;

        private readonly DateTimeZone _timeZone;

        private byte[]? pictureData = null;

        public MainForm(
            IOptions<EpaperRenderingOptions> renderingOptions,
            IOptions<OpenWeatherOptions> openWeatherOptions,
            IEpaperSocketClient epaperSocketClient,
            EpaperDataService epaperDataService
        )
        {
            InitializeComponent();

            _openWeatherOptions = openWeatherOptions.Value;

            _epaperDataService =
                epaperDataService ?? throw new ArgumentNullException(nameof(epaperDataService));
            _epaperSocketClient =
                epaperSocketClient ?? throw new ArgumentNullException(nameof(epaperSocketClient));

            _timeZone = DateTimeZoneProviders.Tzdb[renderingOptions.Value.TimeZone];

            _assets = new RenderingAssets(renderingOptions.Value.AssetsPath);

            sendButton.Enabled = true;
            renderButton.Enabled = true;
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = _renderState;
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

                Bitmap bitmap = new (ms);

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
            if (_renderState is null)
            {
                return;
            }

            this.Text = "Rendering...";
            using (var renderer = new GraphicsRenderer(_assets, _timeZone))
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
    }
}
