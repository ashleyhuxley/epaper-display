using System.Text.Json.Serialization;

namespace ElectricFox.HomeAssistant.Model
{
    public class StateHistory
    {
        [JsonPropertyName("entity_id")]
        public string? EntityId { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, object>? Attributes { get; set; } = new();

        [JsonPropertyName("last_changed")]
        public DateTime? LastChanged { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime? LastUpdated { get; set; }
    }
}
