using Ardalis.Specification;
using ElectricFox.HomeAssistantData.Entities;

namespace ElectricFox.HomeAssistantData.Specifications
{
    public class StateSpecification : Specification<State>
    {
        public StateSpecification(string sensorId)
        {
            Query.Where(s => s.SensorId == sensorId);
        }
    }
}
