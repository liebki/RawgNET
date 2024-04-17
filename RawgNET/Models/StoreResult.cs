using Newtonsoft.Json;

namespace RawgNET.Models
{
    public partial class StoreResult
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        [JsonProperty("next")]
        public object Next { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public Store[] StoreList { get; set; }
    }

    public partial class Store
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string Domain { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("games_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GamesCount { get; set; }

        [JsonProperty("image_background", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageBackground { get; set; }

        [JsonProperty("games", NullValueHandling = NullValueHandling.Ignore)]
        public StoreGame[] GameList { get; set; }

        public override string ToString()
        {
            return $"Store: {Id} - {Name} - {Domain} - {Slug} - {GamesCount} - {ImageBackground}";
        }
    }

    public partial class StoreGame
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("added", NullValueHandling = NullValueHandling.Ignore)]
        public long? Added { get; set; }

        public override string ToString()
        {
            return $"Game: {Id} - {Slug} - {Name} - {Added}";
        }
    }
}