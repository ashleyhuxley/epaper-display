using NodaTime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectricFox.HomeAssistant.Model
{
    public class Context
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
    }

    public class Sensor
    {
        [JsonPropertyName("entity_id")]
        public string? EntityId { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("attributes")]
        public Dictionary<string, JsonElement>? Attributes { get; set; }

        [JsonPropertyName("last_changed")]
        public OffsetDateTime LastChanged { get; set; }

        [JsonPropertyName("last_reported")]
        public OffsetDateTime LastReported { get; set; }

        [JsonPropertyName("last_updated")]
        public OffsetDateTime LastUpdated { get; set; }

        [JsonPropertyName("context")]
        public Context? Context { get; set; }
    }
}
