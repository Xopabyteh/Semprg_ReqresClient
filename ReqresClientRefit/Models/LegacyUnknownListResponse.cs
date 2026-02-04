using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LegacyUnknownListResponse
{
    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("per_page")]
    public int PerPage { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("data")]
    public List<LegacyUnknown> Data { get; set; } = new();

    [JsonProperty("support")]
    public Dictionary<string, object>? Support { get; set; }
}
