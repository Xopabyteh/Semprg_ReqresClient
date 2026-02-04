using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class ErrorResponse
{
    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;

    [JsonProperty("message")]
    public string? Message { get; set; }
}
