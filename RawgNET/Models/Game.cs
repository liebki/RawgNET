﻿using Newtonsoft.Json;

namespace RawgNET.Models
{
    public class Game
    {
        public bool AreAchievementsAvailable { get; set; }

        public IEnumerable<Achievement>? Achievements { get; set; }

        public bool AreScreenshotsAvailable { get; set; }

        public IEnumerable<Screenshot>? Screenshots { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("name_original", NullValueHandling = NullValueHandling.Ignore)]
        public string NameOriginal { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("metacritic", NullValueHandling = NullValueHandling.Ignore)]
        public long? Metacritic { get; set; }

        [JsonProperty("metacritic_platforms", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<MetacriticPlatform> MetacriticPlatforms { get; set; }

        [JsonProperty("released", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Released { get; set; }

        [JsonProperty("tba", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Tba { get; set; }

        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Updated { get; set; }

        [JsonProperty("background_image", NullValueHandling = NullValueHandling.Ignore)]
        public Uri BackgroundImage { get; set; }

        [JsonProperty("background_image_additional", NullValueHandling = NullValueHandling.Ignore)]
        public Uri BackgroundImageAdditional { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Website { get; set; }

        [JsonProperty("rating", NullValueHandling = NullValueHandling.Ignore)]
        public double? Rating { get; set; }

        [JsonProperty("rating_top", NullValueHandling = NullValueHandling.Ignore)]
        public long? RatingTop { get; set; }

        [JsonProperty("ratings", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Rating> Ratings { get; set; }

        [JsonProperty("reactions", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> Reactions { get; set; }

        [JsonProperty("added", NullValueHandling = NullValueHandling.Ignore)]
        public long? Added { get; set; }

        [JsonProperty("added_by_status", NullValueHandling = NullValueHandling.Ignore)]
        public AddedByStatus AddedByStatus { get; set; }

        [JsonProperty("playtime", NullValueHandling = NullValueHandling.Ignore)]
        public long? Playtime { get; set; }

        [JsonProperty("screenshots_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ScreenshotsCount { get; set; }

        [JsonProperty("movies_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? MoviesCount { get; set; }

        [JsonProperty("creators_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreatorsCount { get; set; }

        [JsonProperty("achievements_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? AchievementsCount { get; set; }

        [JsonProperty("parent_achievements_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentAchievementsCount { get; set; }

        [JsonProperty("reddit_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedditUrl { get; set; }

        [JsonProperty("reddit_name", NullValueHandling = NullValueHandling.Ignore)]
        public string RedditName { get; set; }

        [JsonProperty("reddit_description", NullValueHandling = NullValueHandling.Ignore)]
        public string RedditDescription { get; set; }

        [JsonProperty("reddit_logo", NullValueHandling = NullValueHandling.Ignore)]
        public Uri RedditLogo { get; set; }

        [JsonProperty("reddit_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? RedditCount { get; set; }

        [JsonProperty("twitch_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? TwitchCount { get; set; }

        [JsonProperty("youtube_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? YoutubeCount { get; set; }

        [JsonProperty("reviews_text_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReviewsTextCount { get; set; }

        [JsonProperty("ratings_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? RatingsCount { get; set; }

        [JsonProperty("suggestions_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? SuggestionsCount { get; set; }

        [JsonProperty("alternative_names", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> AlternativeNames { get; set; }

        [JsonProperty("metacritic_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri MetacriticUrl { get; set; }

        [JsonProperty("parents_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentsCount { get; set; }

        [JsonProperty("additions_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? AdditionsCount { get; set; }

        [JsonProperty("game_series_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GameSeriesCount { get; set; }

        [JsonProperty("user_game")]
        public object UserGame { get; set; }

        [JsonProperty("reviews_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ReviewsCount { get; set; }

        [JsonProperty("saturated_color", NullValueHandling = NullValueHandling.Ignore)]
        public string SaturatedColor { get; set; }

        [JsonProperty("dominant_color", NullValueHandling = NullValueHandling.Ignore)]
        public string DominantColor { get; set; }

        [JsonProperty("parent_platforms", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ParentPlatform> ParentPlatforms { get; set; }

        [JsonProperty("platforms", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<PlatformElement> Platforms { get; set; }

        [JsonProperty("stores", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<GameStore> Stores { get; set; }

        [JsonProperty("developers", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Developer> Developers { get; set; }

        [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Developer> Genres { get; set; }

        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Developer> Tags { get; set; }

        [JsonProperty("publishers", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Developer> Publishers { get; set; }

        [JsonProperty("esrb_rating", NullValueHandling = NullValueHandling.Ignore)]
        public EsrbRating EsrbRating { get; set; }

        [JsonProperty("clip")]
        public object Clip { get; set; }

        [JsonProperty("description_raw", NullValueHandling = NullValueHandling.Ignore)]
        public string DescriptionRaw { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(AreAchievementsAvailable)}={AreAchievementsAvailable}, {nameof(Achievements)}={Achievements}, {nameof(Id)}={Id}, {nameof(Slug)}={Slug}, {nameof(Name)}={Name}, {nameof(NameOriginal)}={NameOriginal}, {nameof(Description)}={Description}, {nameof(Metacritic)}={Metacritic}, {nameof(MetacriticPlatforms)}={MetacriticPlatforms}, {nameof(Released)}={Released}, {nameof(Tba)}={Tba}, {nameof(Updated)}={Updated}, {nameof(BackgroundImage)}={BackgroundImage}, {nameof(BackgroundImageAdditional)}={BackgroundImageAdditional}, {nameof(Website)}={Website}, {nameof(Rating)}={Rating}, {nameof(RatingTop)}={RatingTop}, {nameof(Ratings)}={Ratings}, {nameof(Reactions)}={Reactions}, {nameof(Added)}={Added}, {nameof(AddedByStatus)}={AddedByStatus}, {nameof(Playtime)}={Playtime}, {nameof(ScreenshotsCount)}={ScreenshotsCount}, {nameof(MoviesCount)}={MoviesCount}, {nameof(CreatorsCount)}={CreatorsCount}, {nameof(AchievementsCount)}={AchievementsCount}, {nameof(ParentAchievementsCount)}={ParentAchievementsCount}, {nameof(RedditUrl)}={RedditUrl}, {nameof(RedditName)}={RedditName}, {nameof(RedditDescription)}={RedditDescription}, {nameof(RedditLogo)}={RedditLogo}, {nameof(RedditCount)}={RedditCount}, {nameof(TwitchCount)}={TwitchCount}, {nameof(YoutubeCount)}={YoutubeCount}, {nameof(ReviewsTextCount)}={ReviewsTextCount}, {nameof(RatingsCount)}={RatingsCount}, {nameof(SuggestionsCount)}={SuggestionsCount}, {nameof(AlternativeNames)}={AlternativeNames}, {nameof(MetacriticUrl)}={MetacriticUrl}, {nameof(ParentsCount)}={ParentsCount}, {nameof(AdditionsCount)}={AdditionsCount}, {nameof(GameSeriesCount)}={GameSeriesCount}, {nameof(UserGame)}={UserGame}, {nameof(ReviewsCount)}={ReviewsCount}, {nameof(SaturatedColor)}={SaturatedColor}, {nameof(DominantColor)}={DominantColor}, {nameof(ParentPlatforms)}={ParentPlatforms}, {nameof(Platforms)}={Platforms}, {nameof(Stores)}={Stores}, {nameof(Developers)}={Developers}, {nameof(Genres)}={Genres}, {nameof(Tags)}={Tags}, {nameof(Publishers)}={Publishers}, {nameof(EsrbRating)}={EsrbRating}, {nameof(Clip)}={Clip}, {nameof(DescriptionRaw)}={DescriptionRaw}}}";
        }
    }

    public partial class AddedByStatus
    {
        [JsonProperty("yet", NullValueHandling = NullValueHandling.Ignore)]
        public long? Yet { get; set; }

        [JsonProperty("owned", NullValueHandling = NullValueHandling.Ignore)]
        public long? Owned { get; set; }

        [JsonProperty("beaten", NullValueHandling = NullValueHandling.Ignore)]
        public long? Beaten { get; set; }

        [JsonProperty("toplay", NullValueHandling = NullValueHandling.Ignore)]
        public long? Toplay { get; set; }

        [JsonProperty("dropped", NullValueHandling = NullValueHandling.Ignore)]
        public long? Dropped { get; set; }

        [JsonProperty("playing", NullValueHandling = NullValueHandling.Ignore)]
        public long? Playing { get; set; }
    }

    public partial class Developer
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("games_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GamesCount { get; set; }

        [JsonProperty("image_background", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageBackground { get; set; }

        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string Domain { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public Language? Language { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public partial class EsrbRating
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }
    }

    public partial class MetacriticPlatform
    {
        [JsonProperty("metascore", NullValueHandling = NullValueHandling.Ignore)]
        public long? Metascore { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public MetacriticPlatformPlatform Platform { get; set; }
    }

    public partial class MetacriticPlatformPlatform
    {
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public long? Platform { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }
    }

    public partial class ParentPlatform
    {
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public EsrbRating Platform { get; set; }
    }

    public partial class PlatformElement
    {
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public PlatformPlatform Platform { get; set; }

        [JsonProperty("released_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ReleasedAt { get; set; }

        [JsonProperty("requirements", NullValueHandling = NullValueHandling.Ignore)]
        public Requirements Requirements { get; set; }
    }

    public partial class PlatformPlatform
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("image")]
        public object Image { get; set; }

        [JsonProperty("year_end")]
        public object YearEnd { get; set; }

        [JsonProperty("year_start")]
        public object YearStart { get; set; }

        [JsonProperty("games_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? GamesCount { get; set; }

        [JsonProperty("image_background", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageBackground { get; set; }
    }

    public partial class Requirements
    {
        private Dictionary<string, string> MinimumValues = new();

        private Dictionary<string, string> RecommendedValues = new();

        [JsonProperty("minimum", NullValueHandling = NullValueHandling.Ignore)]
        public string Minimum { get; set; }

        [JsonProperty("recommended", NullValueHandling = NullValueHandling.Ignore)]
        public string Recommended { get; set; }

        /// <summary>
        /// Get the components and values - Use with caution, since the strings are parsed it could throw an exception
        /// </summary>
        public Dictionary<string, string> MinimumParsed()
        {
            if (!string.IsNullOrEmpty(this.Minimum))
            {
                if (this.Minimum.Contains("OS:"))
                {
                    this.MinimumValues.Add("OS", ClearString(this.Minimum.Split("OS:")[1].Split("Processor:")[0]));
                }
                if (this.Minimum.Contains("Processor:"))
                {
                    this.MinimumValues.Add("CPU", ClearString(this.Minimum.Split("Processor:")[1].Split("Memory:")[0]));
                }
                if (this.Minimum.Contains("Memory:"))
                {
                    this.MinimumValues.Add("RAM", ClearString(this.Minimum.Split("Memory:")[1].Split("Graphics:")[0]));
                }
                if (this.Minimum.Contains("Graphics:"))
                {
                    string GpuValue = ClearString(this.Minimum.Split("Graphics:")[1].Split("Storage:")[0]);
                    if (GpuValue.Contains("DirectX:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.MinimumValues.Add("GPU", GpuValue.Split("DirectX:")[0]);
                    }
                    else
                    {
                        this.MinimumValues.Add("GPU", GpuValue);
                    }
                }
                if (this.Minimum.Contains("Storage:"))
                {
                    this.MinimumValues.Add("STORAGE", ClearString(this.Minimum.Split("Storage:")[1].Split("available space")[0].Trim()));
                }
            }

            return MinimumValues;
        }

        /// <summary>
        /// Get the components and values - Use with caution, since the strings are parsed it could throw an exception
        /// </summary>
        public Dictionary<string, string> RecommendedParsed()
        {
            if (!string.IsNullOrEmpty(this.Recommended))
            {
                if (this.Recommended.Contains("OS:"))
                {
                    this.RecommendedValues.Add("OS", ClearString(this.Recommended.Split("OS:")[1].Split("Processor:")[0]));
                }
                if (this.Recommended.Contains("Processor:"))
                {
                    this.RecommendedValues.Add("CPU", ClearString(this.Recommended.Split("Processor:")[1].Split("Memory:")[0]));
                }
                if (this.Recommended.Contains("Memory:"))
                {
                    this.RecommendedValues.Add("RAM", ClearString(this.Recommended.Split("Memory:")[1].Split("Graphics:")[0]));
                }
                if (this.Recommended.Contains("Graphics:"))
                {
                    string GpuValue = ClearString(this.Recommended.Split("Graphics:")[1].Split("Storage:")[0]);
                    if (GpuValue.Contains("DirectX:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.RecommendedValues.Add("GPU", GpuValue.Split("DirectX:")[0]);
                    }
                    else
                    {
                        this.RecommendedValues.Add("GPU", GpuValue);
                    }
                }
                if (this.Recommended.Contains("Storage:"))
                {
                    this.RecommendedValues.Add("STORAGE", ClearString(this.Recommended.Split("Storage:")[1].Split("available space")[0].Trim()));
                }
            }
            return RecommendedValues;
        }

        private static string ClearString(string stringInput)
        {
            return stringInput.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(Environment.NewLine, string.Empty);
        }
    }

    public partial class GameStore
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("store", NullValueHandling = NullValueHandling.Ignore)]
        public Developer StoreStore { get; set; }

    }

    public enum Language
    { Eng };
}