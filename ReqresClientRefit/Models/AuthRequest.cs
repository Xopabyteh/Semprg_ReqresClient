using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class AuthRequest
{
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
}
