using Microsoft.Extensions.Logging;
using Moq;

namespace ElectricFox.HomeAssistant.IntegrationTests
{
    public class Tests
    {
        HomeAssistantClient _client;

        [SetUp]
        public void Setup()
        {
            var options = new HomeAssistantOptions
            {
                ApiToken = "",
                BaseUrl = ""
            };

            var client = new HttpClient();

            var logger = new Mock<ILogger<HomeAssistantClient>>();

            _client = new HomeAssistantClient(options, client, logger.Object);
        }

        [Test]
        public async Task GetState_ReturnsExpectedAttributes()
        {
            var sensor = await _client.GetSensorState("sensor.average_indoor_temperature", CancellationToken.None);
            Assert.IsNotNull(sensor);
            Assert.IsNotNull(sensor.Attributes);
            Assert.That(sensor?.Attributes["StateClass"], Is.EqualTo("measurement"));
            Assert.That(sensor?.Attributes["FriendlyName"], Is.EqualTo("Average Indoor Temperature"));
        }
    }
}