using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class AppUser
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("last_login_at")]
    public DateTime? LastLoginAt { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}
