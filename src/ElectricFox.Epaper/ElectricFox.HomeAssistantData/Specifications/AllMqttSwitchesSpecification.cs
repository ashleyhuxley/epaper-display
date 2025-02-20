using Ardalis.Specification;
using ElectricFox.HomeAssistantData.Entities;

namespace ElectricFox.HomeAssistantData.Specifications
{
    public class AllMqttSwitchesSpecification : Specification<MqttSwitch>
    {
        public AllMqttSwitchesSpecification()
        {
            Query.Where(t => true).Include(s => s.Device);
        }
    }
}
