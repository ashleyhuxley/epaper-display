using System.ComponentModel.DataAnnotations;

namespace ElectricFox.HomeAssistantData.Entities
{
    public class MqttSensor
    {
        [Key]
        public int SensorId { get; init; }
        public string? Name { get; init; }
        public string? UnitOfMeasurement { get; init; }
        public string? UniqueId { get; init; }
        public string? StateTopic { get; init; }
        public string? IconId { get; init; }

        public virtual MqttDevice? Device { get; set; }
    }
}
