using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LegacyUnknownResponse
{
    [JsonProperty("data")]
    public LegacyUnknown Data { get; set; } = new();

    [JsonProperty("support")]
    public Dictionary<string, object>? Support { get; set; }
}
