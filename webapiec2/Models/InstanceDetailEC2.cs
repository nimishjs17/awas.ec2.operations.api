using Newtonsoft.Json;

namespace webapiec2.Models
{
    public class InstanceDetailEC2
    {
        [JsonProperty("_instanceId")]
        public string _instanceId { get; set; }

        [JsonProperty("_isntanceName")]
        public string _isntanceName { get; set; }

        public string _ipAddress { get; set; }

        [JsonProperty("_code")]
        public int? _code { get; set; }

        [JsonProperty("_state")]
        public string _state { get; set; }
        //     0
        //     :
        //     pending
        //     16
        //     :
        //     running
        //     32
        //     :
        //     shutting-down
        //     48
        //     :
        //     terminated
        //     64
        //     :
        //     stopping
        //     80
        //     :
        //     stopped
    }
}
