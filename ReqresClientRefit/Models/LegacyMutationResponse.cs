using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LegacyMutationResponse
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("job")]
    public string? Job { get; set; }

    [JsonProperty("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
