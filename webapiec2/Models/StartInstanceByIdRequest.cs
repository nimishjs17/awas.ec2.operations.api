using Newtonsoft.Json;
using System.Collections.Generic;

namespace webapiec2.Models
{
    public class StartInstanceByIdRequest
    {
        [JsonProperty("_instanceIds")]
        public List<string> _instanceIds { get; set; }

        [JsonProperty("_authToken")]
        public string AuthToken { get; set; }

    }
}
