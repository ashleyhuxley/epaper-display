using System.Text.Json.Serialization;

namespace ElectricFox.HomeAssistant.Model.Climate
{
    public class Attributes
    {
        [JsonPropertyName("hvac_modes")]
        public List<string> HvacModes { get; set; }

        [JsonPropertyName("min_temp")]
        public int? MinTemp { get; set; }

        [JsonPropertyName("max_temp")]
        public int? MaxTemp { get; set; }

        [JsonPropertyName("target_temp_step")]
        public double? TargetTempStep { get; set; }

        [JsonPropertyName("preset_modes")]
        public List<string> PresetModes { get; set; }

        [JsonPropertyName("current_temperature")]
        public double? CurrentTemperature { get; set; }

        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("hvac_action")]
        public string HvacAction { get; set; }

        [JsonPropertyName("preset_mode")]
        public string PresetMode { get; set; }

        [JsonPropertyName("friendly_name")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("supported_features")]
        public int? SupportedFeatures { get; set; }
    }
}
