using System.ComponentModel.DataAnnotations;

namespace ElectricFox.HomeAssistantData.Entities
{
    public class MqttDevice
    {
        [Key]
        public int DeviceId { get; set; }
        public string? Name { get; set; }
        public string? ConfigUrl { get; set; }
        public string? Identifiers { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? SuggestedArea { get; set; }
    }
}
