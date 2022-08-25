using Newtonsoft.Json;
using System;

namespace webapiec2.Models
{
    public class EC2Response
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; } = true;

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; } = null;

        [JsonProperty("response")]
        public dynamic Response { get; set; } = null;

        //[JsonProperty("innnerException")]
       // public Exception InnnerException { get; set; } = null;
    }
}
