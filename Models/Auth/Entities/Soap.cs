using Newtonsoft.Json;

namespace proxy_net.Models.Auth.Entities
{
    public class SoapResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Jwt { get; set; }
    }
}
