using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LegacyUserResponse
{
    [JsonProperty("data")]
    public LegacyUser Data { get; set; } = new();

    [JsonProperty("support")]
    public Dictionary<string, object>? Support { get; set; }
}
