using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class LoginResponse
{
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;
}
