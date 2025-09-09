using ElectricFox.HomeAssistant.Model;
using ElectricFox.HomeAssistant.Model.Climate;

namespace ElectricFox.HomeAssistant
{
    public interface IHomeAssistantClient
    {
        Task<Sensor?> GetSensorState(string sensorId, CancellationToken cancellationToken);
        Task<Climate?> GetClimate(string climateId, CancellationToken cancellationToken);
        Task<List<StateHistory>?> GetSensorStateHistory(string sensorId, DateTime from, CancellationToken cancellationToken);
    }
}
