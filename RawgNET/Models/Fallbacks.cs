using Newtonsoft.Json;

namespace RawgNET.Models
{
    internal class GameFallback
    {
        [JsonProperty("redirect")]
        public bool Redirect { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    internal class NotFound
    {
        [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }
    }
}