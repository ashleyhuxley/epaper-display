using Ardalis.Specification;
using ElectricFox.HomeAssistantData.Entities;

namespace ElectricFox.HomeAssistantData.Specifications
{
    public class AllMqttSensorsSpecification : Specification<MqttSensor>
    {
        public AllMqttSensorsSpecification()
        {
            Query.Where(t => true).Include(s => s.Device);
        }
    }
}
