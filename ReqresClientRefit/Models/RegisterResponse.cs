using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class RegisterResponse
{
    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;
}
