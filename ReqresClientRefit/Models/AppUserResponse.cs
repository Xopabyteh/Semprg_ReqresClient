using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class AppUserResponse
{
    [JsonProperty("data")]
    public AppUser Data { get; set; } = new();
}
