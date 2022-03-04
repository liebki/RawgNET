using Newtonsoft.Json;

namespace RawgNET
{
    internal class GameFallback
    {
        [JsonProperty("redirect")]
        public bool Redirect { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }
}