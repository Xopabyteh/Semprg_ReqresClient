using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class CollectionListResponse
{
    [JsonProperty("data")]
    public List<Collection> Data { get; set; } = new();
}
