using ElectricFox.HomeAssistantData.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectricFox.HomeAssistantData
{
    public class HomeAssistantContext(DbContextOptions<HomeAssistantContext> options)
        : DbContext(options)
    {
        public DbSet<State> States { get; init; }
        public DbSet<MqttSensor> MqttSensors { get; init; }
        public DbSet<MqttSwitch> MqttSwitches { get; init; }
        public DbSet<MqttDevice> MqttDevices { get; init; }
    }
}
