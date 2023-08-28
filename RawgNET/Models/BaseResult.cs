using Newtonsoft.Json;
using System;
using System.Linq;

namespace RawgNET.Models
{
    public class BaseResult<TItem>
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("next", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Next { get; set; }

        [JsonProperty("previous")]
        public Uri Previous { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<TItem> Items { get; set; }
    }
}
