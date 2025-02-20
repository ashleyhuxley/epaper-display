using System.Text.Json.Serialization;

namespace ElectricFox.HomeAssistant.Model.Climate
{
    public class Context
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("parent_id")]
        public object ParentId { get; set; }

        [JsonPropertyName("user_id")]
        public object UserId { get; set; }
    }
}
