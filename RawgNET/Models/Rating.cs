using System;
using System.Linq;

using Newtonsoft.Json;

namespace RawgNET.Models
{
    public partial class Rating
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("percent", NullValueHandling = NullValueHandling.Ignore)]
        public double? Percent { get; set; }
    }
}
