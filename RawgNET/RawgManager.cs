using RawgNET.Models;
using Newtonsoft.Json;

namespace RawgNET
{
    internal class RawgManager
    {
        #region Variables

        private const string RawgApiBaseUrl = "https://rawg.io/api";

        #endregion Variables

        #region Creator

        internal static async Task<Creator> GetCreator(string rawgkey, string creatorid)
        {
            string RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, QueryType.Creator, creatorid);
            string JsonResponse = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }

            Creator creator = DeserializeJsonToObject(JsonResponse, QueryType.Creator);
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
            string JsonResponse = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }

            if (JsonResponse.Contains("detail"))
            {
                NotFound CreatorNotFound = DeserializeJsonToObject(JsonResponse, QueryType.NotFound);
                if (CreatorNotFound.Detail.Contains("Not found."))
                {
                    return false;
                }
            }
            return true;
        }

        internal static async Task<List<Creator>> GetAllCreators(string rawgkey, int maxresults = 0)
        {
            CreatorResult creatorQuery = await QueryCreators(rawgkey);
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

        /// <summary>
        /// The first query to see if there are any creators
        /// </summary>
        /// <param name="rawgkey"></param>
        /// <returns></returns>
        private static async Task<CreatorResult> QueryCreators(string rawgkey)
        {
            string RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, QueryType.CreatorList);
            string JsonResponse = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.CreatorList);
        }

        /// <summary>
        /// Recursive method, to get every creator
        /// </summary>
        /// <returns>List of creators</returns>
        private static async Task<List<Creator>> QueryAllCreators(CreatorResult creatorQuery, List<Creator> CreatorList, int leftresults = 0)
        {
            if (leftresults > 0)
            {
                CreatorResult CreatorQuery = await QueryMoreCreators(creatorQuery.Next.AbsoluteUri);
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

        private static async Task<CreatorResult> QueryMoreCreators(string page)
        {
            string JsonResponse = string.Empty;
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(page);
                TaskResponse.Wait();

                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.CreatorList);
        }

        #endregion Creator

        #region Screenshots

        /// <summary>
        /// Recursive method, to get every screenshot of a game
        /// </summary>
        /// <param name="gameScreenshotQuery">The previous Screenshot-Object</param>
        /// <param name="ScreenshotList">The previous Screenshot-List</param>
        /// <returns></returns>
        private static async Task<List<Screenshot>> QueryAllScreenshots(ScreenshotResult gameScreenshotQuery, List<Screenshot> ScreenshotList)
        {
            ScreenshotResult ScreenshotQuery = await QueryMoreScreenshots(gameScreenshotQuery.Next.AbsoluteUri);
            List<Screenshot> AllAchievements = ScreenshotList;

            AllAchievements.AddRange(ScreenshotQuery.Screenshots);
            if (ScreenshotQuery.Next?.AbsoluteUri.Contains("page=") == true)
            {
                QueryAllScreenshots(ScreenshotQuery, AllAchievements);
            }
            return AllAchievements;
        }

        private static async Task<ScreenshotResult> QueryScreenshots(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Screenshot);
            string JsonResponse = string.Empty;
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Screenshot);
        }

        private static async Task<ScreenshotResult> QueryMoreScreenshots(string page)
        {
            string JsonResponse = string.Empty;
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(page);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Screenshot);
        }

        #endregion Screenshots

        #region Achievements

        /// <summary>
        /// Recursive method, to get every achievement of a game
        /// </summary>
        /// <param name="gameAchievementQuery">The previous Achievement-Object</param>
        /// <param name="AchievementList">The previous Achievement-List</param>
        /// <returns></returns>
        private static async Task<List<Achievement>> QueryAllAchievements(AchievementResult gameAchievementQuery, List<Achievement> AchievementList)
        {
            AchievementResult achievementQuery = await QueryMoreAchievements(gameAchievementQuery.Next.AbsoluteUri);
            List<Achievement> AllAchievements = AchievementList;

            AllAchievements.AddRange(achievementQuery.Achievements);
            if (achievementQuery.Next?.AbsoluteUri.Contains("page=") == true)
            {
                QueryAllAchievements(achievementQuery, AllAchievements);
            }
            return AllAchievements;
        }

        private static async Task<AchievementResult> QueryMoreAchievements(string page)
        {
            string JsonResponse = string.Empty;
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(page);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Achievement);
        }


        private static async Task<AchievementResult> QueryAchievements(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Achievement);
            string JsonResponse = string.Empty;
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Achievement);
        }



        #endregion Achievements

        #region Game

        /// <summary>
        /// The main method, where we get our data from
        /// </summary>
        /// <param name="name">Name of the game we'd like to query</param>
        /// <param name="rawgkey">Your API-Key</param>
        /// <param name="getAchievements">If we want to query for the achievements (takes a second longer)</param>
        /// <returns></returns>
        internal static async Task<Game> RawgRequest(string name, string rawgkey, bool getAchievements, bool getScreenshots)
        {
            Game? GameReturnValue = null;
            Game GameQueryResult = await QueryGame(name, rawgkey);

            if (GameQueryResult.BackgroundImage == null)
            {
                GameFallback GameFallbackquery = await QueryFallback(name, rawgkey);

                if (GameFallbackquery.Redirect)
                {
                    Game GameRequeryResult = await QueryGame(GameFallbackquery.Slug, rawgkey);
                    GameQueryResult = GameRequeryResult;
                }
            }

            if (!object.Equals(GameQueryResult.BackgroundImage, null))
            {
                GameReturnValue = GameQueryResult;
                if (object.Equals(GameReturnValue.Metacritic, null))
                {
                    GameReturnValue.Metacritic = 0;
                }

                GameReturnValue.AchievementsAvailable = false;
                GameReturnValue.Achievements = new();

                if (getAchievements)
                {
                    GetAndParse(rawgkey, GameReturnValue, GameQueryResult, QueryType.Achievement);
                }

                GameReturnValue.ScreenshotsAvailable = false;
                GameReturnValue.Screenshots = new();

                if (getScreenshots)
                {
                    GetAndParse(rawgkey, GameReturnValue, GameQueryResult, QueryType.Screenshot);
                }
            }
            return GameReturnValue;
        }

        private static async Task GetAndParse(string rawgkey, Game? GameReturnValue, Game GameQueryResult, QueryType type)
        {
            if (type == QueryType.Achievement)
            {
                AchievementResult gameAchievementQuery = await QueryAchievements(GameReturnValue.Slug, rawgkey);

                if (gameAchievementQuery.Count > 0)
                {
                    List<Achievement> AchievementList = new();
                    GameReturnValue.AchievementsAvailable = true;

                    AchievementList.AddRange(gameAchievementQuery.Achievements);
                    if (gameAchievementQuery.Next?.AbsoluteUri.Contains("page=") == true)
                    {
                        AchievementList.AddRange(await QueryAllAchievements(gameAchievementQuery, new()));
                    }
                    GameReturnValue.Achievements = AchievementList;
                }
            }
            else
            {
                ScreenshotResult gameScreenshotQuery = await QueryScreenshots(GameReturnValue.Slug, rawgkey);
                if (gameScreenshotQuery.Count > 0)
                {
                    List<Screenshot> ScreenshotList = new();
                    GameReturnValue.ScreenshotsAvailable = true;

                    ScreenshotList.AddRange(gameScreenshotQuery.Screenshots);
                    if (gameScreenshotQuery.Next?.AbsoluteUri.Contains("page=") == true)
                    {
                        ScreenshotList.AddRange(await QueryAllScreenshots(gameScreenshotQuery, new()));
                    }
                    GameReturnValue.Screenshots = ScreenshotList;
                }
            }
        }

        /// <summary>
        /// A method, to see if a game exists in the first place
        /// </summary>
        /// <param name="name">Name of the game we'd like to query</param>
        /// <param name="rawgkey">Your API-Key</param>
        /// <returns></returns>
        internal static async Task<bool> RawgRequestGameExists(string name, string rawgkey)
        {
            Game GameQueryResult = await QueryGame(name, rawgkey);

            if (object.Equals(GameQueryResult.BackgroundImage, null) && !string.IsNullOrEmpty(GameQueryResult.Slug))
            {
                Game SlugTry = await QueryGame(GameQueryResult.Slug, rawgkey);
                if (SlugTry.Id != null && SlugTry.BackgroundImage != null)
                {
                    return true;
                }
            }
            return false;
        }
        private static async Task<GameFallback> QueryFallback(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Game);
            string JsonResponse = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.GameFallback);
        }

        private static async Task<Game> QueryGame(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Game);
            string JsonResponse = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Game);
        }

        #endregion Game

        #region Stuff

        private static string CreateNonGameQueryUrl(string rawgkey, QueryType type, string creatorid = "")
        {
            string reqUrl = RawgApiBaseUrl;
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

        private static string CreateQueryUrl(string gamename, string rawgkey, QueryType type)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();

            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }

            string reqUrl = $"{RawgApiBaseUrl}/games/{GameName}";
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

        private static dynamic? DeserializeJsonToObject(string json, QueryType type)
        {
            if (!object.Equals(json, null))
            {
                if (type == QueryType.Game)
                {
                    return JsonConvert.DeserializeObject<Game>(json);
                }
                else if (type == QueryType.GameFallback)
                {
                    return JsonConvert.DeserializeObject<GameFallback>(json);
                }
                else if (type == QueryType.Achievement)
                {
                    return JsonConvert.DeserializeObject<AchievementResult>(json);
                }
                else if (type == QueryType.CreatorList)
                {
                    return JsonConvert.DeserializeObject<CreatorResult>(json);
                }
                else if (type == QueryType.Creator)
                {
                    return JsonConvert.DeserializeObject<Creator>(json);
                }
                else if (type == QueryType.NotFound)
                {
                    return JsonConvert.DeserializeObject<NotFound>(json);
                }
                else
                {
                    return JsonConvert.DeserializeObject<ScreenshotResult>(json);
                }
            }
            return null;
        }

        #endregion Stuff

    }
}