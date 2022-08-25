using Newtonsoft.Json;

namespace webapiec2.Models
{
    public class ListRequest
    {
        [JsonProperty("_authToken")]
        public string AuthToken { get; set; }
    }
}
