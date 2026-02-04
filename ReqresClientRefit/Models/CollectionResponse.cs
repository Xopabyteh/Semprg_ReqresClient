using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class CollectionResponse
{
    [JsonProperty("data")]
    public Collection Data { get; set; } = new();
}
