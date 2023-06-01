using Newtonsoft.Json;
using RawgNET.Models;

namespace RawgNET
{
    internal class RawgManager
    {
        protected RawgManager()
        { }

        private const string BaseUrl = "https://rawg.io/api";

        #region Creator

        internal static async Task<Creator> GetCreator(string rawgkey, string creatorid)
        {
            string RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, QueryType.Creator, creatorid);
            string WebResponseAsJson = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                WebResponseAsJson = await TaskResponse.Result.Content.ReadAsStringAsync();
            }

            Creator creator = DeserializeJsonToObject<Creator>(WebResponseAsJson);
            creator.IsCreatorExisting = true;

            if (creator.Id == null)
            {
                creator.IsCreatorExisting = false;
            }

            return creator;
        }

        internal static async Task<bool> IsCreatorExisting(string rawgkey, string creatorid)
        {
            string RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, QueryType.Creator, creatorid);
            string WebResponseAsJson = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                WebResponseAsJson = await TaskResponse.Result.Content.ReadAsStringAsync();
            }

            if (WebResponseAsJson.Contains("detail"))
            {
                NotFound CreatorNotFound = DeserializeJsonToObject<NotFound>(WebResponseAsJson);
                if (CreatorNotFound.Detail.Contains("Not found."))
                {
                    return false;
                }
            }
            return true;
        }

        internal static async Task<List<Creator>> GetAllCreators(string rawgkey, int maxresults = 0)
        {
            CreatorResult creatorQuery = await Query<CreatorResult>(rawgkey, QueryType.CreatorList);
            List<Creator> creators = new();

            if (creatorQuery.Count > 0)
            {
                int CreatorAddCount = 0;
                foreach (Creator cr in creatorQuery.Creators)
                {
                    if (CreatorAddCount < maxresults)
                    {
                        CreatorAddCount++;
                        creators.Add(cr);
                    }
                }

                if (creatorQuery.Next?.AbsoluteUri.Contains("page=") == true)
                {
                    List<Creator> MoreCreators = await QueryAllCreators(creatorQuery, new(), maxresults - creators.Count);
                    if (MoreCreators.Count > 0)
                    {
                        creators.AddRange(MoreCreators);
                    }
                }
            }

            return creators;
        }

        private static async Task<List<Creator>> QueryAllCreators(CreatorResult creatorQuery, List<Creator> CreatorList, int leftresults = 0)
        {
            if (leftresults > 0)
            {
                CreatorResult CreatorQuery = await QueryMorePages<CreatorResult>(creatorQuery.Next.AbsoluteUri, QueryType.CreatorList);
                List<Creator> AllCreators = CreatorList;

                foreach (Creator cr in CreatorQuery.Creators)
                {
                    if (leftresults > 0)
                    {
                        leftresults--;
                        AllCreators.Add(cr);
                    }
                }

                if (CreatorQuery.Next?.AbsoluteUri.Contains("page=") == true)
                {
                    await QueryAllCreators(CreatorQuery, AllCreators, leftresults);
                }
                return AllCreators;
            }
            return new();
        }

        #endregion Creator

        #region Game

        internal static async Task<Game> RawgRequest(string name, string rawgkey, bool getAchievements, bool getScreenshots)
        {
            Game GameReturnValue = new();
            Game GameQueryResult = await Query<Game>(rawgkey, QueryType.Game, gamename: name);

            if (GameQueryResult.BackgroundImage == null)
            {
                GameFallback GameFallbackquery = await QueryFallback(name, rawgkey);
                if (GameFallbackquery.Redirect)
                {
                    Game GameRequeryResult = await Query<Game>(rawgkey, QueryType.Game, gamename: GameFallbackquery.Slug);
                    GameQueryResult = GameRequeryResult;
                }
            }

            if (GameQueryResult.BackgroundImage != null)
            {
                GameReturnValue = GameQueryResult;
                if (GameReturnValue.Metacritic == null)
                {
                    GameReturnValue.Metacritic = 0;
                }

                GameReturnValue.AreAchievementsAvailable = false;
                GameReturnValue.Achievements = new();

                if (getAchievements)
                {
                    await GetAndParse(rawgkey, GameReturnValue, QueryType.Achievement);
                }

                GameReturnValue.AreScreenshotsAvailable = false;
                GameReturnValue.Screenshots = new();

                if (getScreenshots)
                {
                    await GetAndParse(rawgkey, GameReturnValue, QueryType.Screenshot);
                }
            }
            return GameReturnValue;
        }

        private static async Task GetAndParse(string rawgkey, Game GameReturnValue, QueryType type)
        {
            if (type == QueryType.Achievement)
            {
                AchievementResult gameAchievementQuery = await Query<AchievementResult>(rawgkey, QueryType.Achievement, gamename: GameReturnValue.Slug);

                if (gameAchievementQuery.Count > 0)
                {
                    List<Achievement> FinalAchievementList = new();
                    GameReturnValue.AreAchievementsAvailable = true;

                    FinalAchievementList.AddRange(gameAchievementQuery.Achievements);
                    if (gameAchievementQuery.Next?.AbsoluteUri.Contains("page=") == true)
                    {
                        dynamic x = await QueryEverything(gameAchievementQuery, new());
                        FinalAchievementList.AddRange(((List<Achievement>)x));
                    }
                    GameReturnValue.Achievements = FinalAchievementList;
                }
            }
            else
            {
                ScreenshotResult gameScreenshotQuery = await Query<ScreenshotResult>(rawgkey, QueryType.Screenshot, gamename: GameReturnValue.Slug);
                if (gameScreenshotQuery.Count > 0)
                {
                    List<Screenshot> FinalScreenshotList = new();
                    GameReturnValue.AreScreenshotsAvailable = true;

                    FinalScreenshotList.AddRange(gameScreenshotQuery.Screenshots);
                    if (gameScreenshotQuery.Next?.AbsoluteUri.Contains("page=") == true)
                    {
                        dynamic x = await QueryEverything(gameScreenshotQuery, new());
                        FinalScreenshotList.AddRange(((List<Screenshot>)x));
                    }
                    GameReturnValue.Screenshots = FinalScreenshotList;
                }
            }
        }

        internal static async Task<bool> RawgRequestGameExists(string name, string rawgkey)
        {
            Game GameQueryResult = await Query<Game>(rawgkey, QueryType.Game, gamename: name);
            if (!string.IsNullOrEmpty(GameQueryResult.Slug))
            {
                if (GameQueryResult.Id == null)
                {
                    Game GameQuerySlug = await Query<Game>(rawgkey, QueryType.Game, gamename: name);
                    if (GameQuerySlug.Id != null)
                    {
                        return true;
                    }
                }
                return true;
            }
            return false;
        }

        private static async Task<GameFallback> QueryFallback(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Game);
            string WebResponseAsJson = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                WebResponseAsJson = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject<GameFallback>(WebResponseAsJson);
        }

        #endregion Game

        #region Stuff

        private static async Task<List<Screenshot>> QueryEverything(ScreenshotResult gameScreenshotQuery, List<Screenshot> ScreenshotList)
        {
            ScreenshotResult ScreenshotQuery = await QueryMorePages<ScreenshotResult>(gameScreenshotQuery.Next.AbsoluteUri, QueryType.Screenshot);
            List<Screenshot> AllAchievements = ScreenshotList;

            AllAchievements.AddRange(ScreenshotQuery.Screenshots);
            if (ScreenshotQuery.Next?.AbsoluteUri.Contains("page=") == true)
            {
                await QueryEverything(ScreenshotQuery, AllAchievements);
            }
            return AllAchievements;
        }

        private static async Task<List<Achievement>> QueryEverything(AchievementResult achievementQuery, List<Achievement> achievementList)
        {
            AchievementResult AchievementQuery = await QueryMorePages<AchievementResult>(achievementQuery.Next.AbsoluteUri, QueryType.Achievement);
            List<Achievement> AllAchievements = achievementList;

            AllAchievements.AddRange(AchievementQuery.Achievements);
            if (AchievementQuery.Next?.AbsoluteUri.Contains("page=") == true)
            {
                await QueryEverything(AchievementQuery, AllAchievements);
            }
            return AllAchievements;
        }

        /// <summary>
        /// Used when more pages or elements need to be queried
        /// </summary>
        /// <param name="page">The next page</param>
        /// <param name="type">The type, basically the object/type of the result</param>
        /// <returns>A suitable object, depending on the QueryType input</returns>
        private static async Task<T> QueryMorePages<T>(string page, QueryType type)
        {
            string WebResponseAsJson = string.Empty;
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(page);
                TaskResponse.Wait();

                WebResponseAsJson = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject<T>(WebResponseAsJson);
        }

        /// <summary>
        /// Used when querying something
        /// </summary>
        /// <param name="rawgkey">The key for the API</param>
        /// <param name="type">The type, basically the object/type of the result</param>
        /// <param name="gamename">The name of the game (optional, only for game queries)</param>
        /// <param name="creatorid">The id of the creator (optional, only for creator queries)</param>
        /// <returns>A suitable object, depending on the QueryType input</returns>
        private static async Task<T> Query<T>(string rawgkey, QueryType type, string gamename = "", string creatorid = "")
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, type);
            string WebResponseAsJson = string.Empty;

            if (type == QueryType.Creator || type == QueryType.CreatorList)
            {
                RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, type, creatorid);
            }

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                WebResponseAsJson = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject<T>(WebResponseAsJson);
        }

        /// <summary>
        /// The generation of the URL for anything unrelated to game-queries (creators, developers etc.)
        /// </summary>
        /// <param name="rawgkey">The key for the API</param>
        /// <param name="type">The type, used to differentiate the URL generation</param>
        /// <param name="creatorid">The id of the creator (optional, only for creator queries)</param>
        /// <returns>The URL, that is used when querying</returns>
        private static string CreateNonGameQueryUrl(string rawgkey, QueryType type, string creatorid = "")
        {
            string reqUrl = BaseUrl;
            if (type == QueryType.CreatorList)
            {
                reqUrl += "/creators";
            }
            else if (type == QueryType.Creator)
            {
                reqUrl += $"/creators/{creatorid}";
            }
            return $"{reqUrl}?key={rawgkey}";
        }

        /// <summary>
        /// The generation of the URL for anything related to game-queries (games, screenshots or achievements)
        /// </summary>
        /// <param name="gamename">The name of the game</param>
        /// <param name="rawgkey">The key for the API</param>
        /// <param name="type">The type, used to differentiate the URL generation</param>
        /// <returns>The URL, that is used when querying</returns>
        private static string CreateQueryUrl(string gamename, string rawgkey, QueryType type)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();

            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }

            string reqUrl = $"{BaseUrl}/games/{GameName}";
            if (type == QueryType.Game)
            {
                reqUrl += $"?search_precise=false&search_exact=false&key={rawgkey}";
            }
            else if (type == QueryType.Achievement)
            {
                reqUrl += $"/achievements?key={rawgkey}";
            }
            else
            {
                reqUrl += $"/screenshots?key={rawgkey}";
            }
            return reqUrl;
        }

        /// <summary>
        /// The input JSON string will be converted to a certain object
        /// </summary>
        /// <param name="json">The result of the query as JSON</param>
        /// <returns>A suitable object, depending on the type</returns>
        private static T DeserializeJsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion Stuff
    }
}