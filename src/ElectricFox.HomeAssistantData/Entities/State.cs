namespace ElectricFox.HomeAssistantData.Entities
{
    public class State
    {
        public int StateId { get; set; }
        public string SensorId { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
