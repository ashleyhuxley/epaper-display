namespace ElectricFox.HomeAssistant
{
    public sealed class HomeAssistantOptions : IHomeAssistantClientOptions
    {
        public required string BaseUrl { get; set; }
        public required string ApiToken { get; set; }
    }
}
