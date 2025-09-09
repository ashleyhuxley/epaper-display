using System.ComponentModel.DataAnnotations;

namespace ElectricFox.HomeAssistantData.Entities
{
    public class MqttSwitch
    {
        [Key]
        public int SwitchId { get; set; }

        public string? Name { get; set; }
        public string? IconId { get; set; }
        public int DeviceId { get; set; }
        public string? EspDeviceName { get; set; }
        public int GpioPin { get; set; }
        public string? UniqueId { get; set; }

        public virtual MqttDevice? Device { get; set; }
    }
}
