using System;

using Newtonsoft.Json;


namespace RawgNET.Models
{
    public partial class CreatorResult
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("next", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Next { get; set; }

        [JsonProperty("previous", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Previous { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public List<Creator> Creators { get; set; }
    }

    public partial class Creator
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Image { get; set; }

        [JsonProperty("image_background", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageBackground { get; set; }

        [JsonProperty("games_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GamesCount { get; set; }

        [JsonProperty("positions", NullValueHandling = NullValueHandling.Ignore)]
        public List<ShortGame> Positions { get; set; }

        [JsonProperty("games", NullValueHandling = NullValueHandling.Ignore)]
        public List<ShortGame> Games { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool IsCreatorExisting { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Name)}={Name}, {nameof(Slug)}={Slug}, {nameof(Image)}={Image}, {nameof(ImageBackground)}={ImageBackground}, {nameof(GamesCount)}={GamesCount.ToString()}, {nameof(Positions)}={Positions}, {nameof(Games)}={Games}, {nameof(IsCreatorExisting)}={IsCreatorExisting.ToString()}}}";
        }
    }

    public partial class ShortGame
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("added", NullValueHandling = NullValueHandling.Ignore)]
        public long? Added { get; set; }
    }
}
