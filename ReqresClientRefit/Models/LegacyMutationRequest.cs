using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LegacyMutationRequest
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("job")]
    public string? Job { get; set; }
}
