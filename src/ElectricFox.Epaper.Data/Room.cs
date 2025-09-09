namespace ElectricFox.Epaper.Data
{
    public sealed record class Room(string Name, string TemperatureSensor, string HumiditySensor)
    {
        public static IEnumerable<Room> AllRooms =>
        [
            new Room("Kitchen", "sensor.mqtt_sensor_kitchen_temp", "sensor.mqtt_sensor_kitchen_humidity"),
            new Room("Living Room", "sensor.mqtt_sensor_lounge_temp", "sensor.mqtt_sensor_lounge_humidity"),
            new Room("Conservatory", "sensor.mqtt_sensor_arboretum_temp", "sensor.mqtt_sensor_arboretum_humidity"),
            new Room("Bridge", "sensor.mqtt_sensor_bridge_temp", "sensor.mqtt_sensor_bridge_humidity"),
            new Room("Fen's Bunk", "sensor.mqtt_sensor_fens_bunk_temp", "sensor.mqtt_sensor_fens_bunk_humidity"),
            new Room("Jay's Bunk", "sensor.mqtt_sensor_jays_bunk_temp", "sensor.mqtt_sensor_jays_bunk_humidity"),
        ];
    }
}
