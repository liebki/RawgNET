﻿using Newtonsoft.Json;

namespace RawgNET.Models
{
    public class ScreenshotResult : BaseResult<Screenshot>
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("previous")]
        public Uri Previous { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Screenshot> Screenshots { get; set; }
    }

    public class Screenshot
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Image { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long? Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("is_deleted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDeleted { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Image)}={Image}, {nameof(Width)}={Width.ToString()}, {nameof(Height)}={Height.ToString()}, {nameof(IsDeleted)}={IsDeleted.ToString()}}}";
        }
    }
}