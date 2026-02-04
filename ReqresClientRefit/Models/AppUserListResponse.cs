using Newtonsoft.Json;

namespace ReqresClientRefit.Models;

public class AppUserListResponse
{
    [JsonProperty("data")]
    public List<AppUser> Data { get; set; } = new();
}
