using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LegacyUnknown
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("year")]
    public int Year { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; } = string.Empty;

    [JsonProperty("pantone_value")]
    public string PantoneValue { get; set; } = string.Empty;
}
