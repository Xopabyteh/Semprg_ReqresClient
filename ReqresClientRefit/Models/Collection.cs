using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class Collection
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("project_id")]
    public string? ProjectId { get; set; }

    [JsonProperty("user_id")]
    public string? UserId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonProperty("schema")]
    public Dictionary<string, object>? Schema { get; set; }

    [JsonProperty("visibility")]
    public string? Visibility { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
