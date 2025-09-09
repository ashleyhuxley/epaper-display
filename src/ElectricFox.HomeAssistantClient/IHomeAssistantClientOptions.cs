namespace ElectricFox.HomeAssistant
{
    public interface IHomeAssistantClientOptions
    {
        string BaseUrl { get; }
        string ApiToken { get; }
    }
}
