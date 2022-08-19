using Newtonsoft.Json;

namespace RawgNET
{
    internal class RawgAccessManager
    {
        private const string RawgApiBaseUrl = "https://rawg.io/api/games/";

        /// <summary>
        /// The main method, where we get our data from
        /// </summary>
        /// <param name="name">Name of the game we'd like to query</param>
        /// <param name="rawgkey">Your API-Key</param>
        /// <param name="getAchievements">If we want to query for the achievements (takes a second longer)</param>
        /// <returns></returns>
        internal static Game RawgRequest(string name, string rawgkey, bool getAchievements, bool getScreenshots)
        {
            Game? GameReturnValue = null;
            Task<Game> GameQueryResult = Task.Run(() => QueryGame(name, rawgkey));
            GameQueryResult.Wait();
            if (object.Equals(GameQueryResult.Result.BackgroundImage, null))
            {
                Task<GameFallback> GameFallbackquery = Task.Run(() => QueryFallback(name, rawgkey));
                GameFallbackquery.Wait();
                if (GameFallbackquery.Result.Redirect)
                {
                    Task<Game> GameRequeryResult = Task.Run(() => QueryGame(GameFallbackquery.Result.Slug, rawgkey));
                    GameQueryResult.Wait();
                    GameQueryResult = GameRequeryResult;
                }
            }
            if (!object.Equals(GameQueryResult.Result.BackgroundImage, null))
            {
                GameReturnValue = GameQueryResult.Result;
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

        private static void GetAndParse(string rawgkey, Game? GameReturnValue, Task<Game> GameQueryResult, QueryType type)
        {
            if (type == QueryType.Achievement)
            {
                Task<AchievementResult> gameAchievementQuery = Task.Run(() => QueryAchievements(GameReturnValue.Slug, rawgkey)); GameQueryResult.Wait();

                if (gameAchievementQuery.Result.Count > 0)
                {
                    List<Achievement> AchievementList = new();
                    GameReturnValue.AchievementsAvailable = true;

                    AchievementList.AddRange(gameAchievementQuery.Result.Achievements);
                    if (gameAchievementQuery.Result.Next?.AbsoluteUri.Contains("page=") == true)
                    {
                        AchievementList.AddRange(QueryAllAchievements(gameAchievementQuery, new()));
                    }
                    GameReturnValue.Achievements = AchievementList;
                }
            }
            else
            {
                Task<ScreenshotResult> gameScreenshotQuery = Task.Run(() => QueryScreenshots(GameReturnValue.Slug, rawgkey));
                GameQueryResult.Wait();

                if (gameScreenshotQuery.Result.Count > 0)
                {
                    List<Screenshot> ScreenshotList = new();
                    GameReturnValue.ScreenshotsAvailable = true;

                    ScreenshotList.AddRange(gameScreenshotQuery.Result.Screenshots);
                    if (gameScreenshotQuery.Result.Next?.AbsoluteUri.Contains("page=") == true)
                    {
                        ScreenshotList.AddRange(QueryAllScreenshots(gameScreenshotQuery, new()));
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
        internal static bool RawgRequestGameExists(string name, string rawgkey)
        {
            Task<Game> GameQueryResult = Task.Run(() => QueryGame(name, rawgkey));
            GameQueryResult.Wait();

            if (object.Equals(GameQueryResult.Result.BackgroundImage, null))
            {
                Task<GameFallback> GameFallbackquery = Task.Run(() => QueryFallback(name, rawgkey));
                GameFallbackquery.Wait();

                if (GameFallbackquery.Result.Redirect)
                {
                    Task<Game> GameRequeryResult = Task.Run(() => QueryGame(GameFallbackquery.Result.Slug, rawgkey));
                    GameQueryResult.Wait();
                    GameQueryResult = GameRequeryResult;

                    if (object.Equals(GameQueryResult.Result.BackgroundImage, null))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Recursive method, to get every screenshot of a game
        /// </summary>
        /// <param name="gameScreenshotQuery">The previous Screenshot-Object</param>
        /// <param name="ScreenshotList">The previous Screenshot-List</param>
        /// <returns></returns>
        private static List<Screenshot> QueryAllScreenshots(Task<ScreenshotResult> gameScreenshotQuery, List<Screenshot> ScreenshotList)
        {
            Task<ScreenshotResult> ScreenshotQuery = Task.Run(() => QueryMoreScreenshots(gameScreenshotQuery.Result.Next.AbsoluteUri));
            ScreenshotQuery.Wait();
            List<Screenshot> AllAchievements = ScreenshotList;
            AllAchievements.AddRange(ScreenshotQuery.Result.Screenshots);
            if (ScreenshotQuery.Result.Next?.AbsoluteUri.Contains("page=") == true)
            {
                QueryAllScreenshots(ScreenshotQuery, AllAchievements);
            }
            return AllAchievements;
        }

        /// <summary>
        /// Recursive method, to get every achievement of a game
        /// </summary>
        /// <param name="gameAchievementQuery">The previous Achievement-Object</param>
        /// <param name="AchievementList">The previous Achievement-List</param>
        /// <returns></returns>
        private static List<Achievement> QueryAllAchievements(Task<AchievementResult> gameAchievementQuery, List<Achievement> AchievementList)
        {
            Task<AchievementResult> achievementQuery = Task.Run(() => QueryMoreAchievements(gameAchievementQuery.Result.Next.AbsoluteUri));
            achievementQuery.Wait();
            List<Achievement> AllAchievements = AchievementList;
            AllAchievements.AddRange(achievementQuery.Result.Achievements);
            if (achievementQuery.Result.Next?.AbsoluteUri.Contains("page=") == true)
            {
                QueryAllAchievements(achievementQuery, AllAchievements);
            }
            return AllAchievements;
        }

        private static async Task<ScreenshotResult> QueryMoreScreenshots(string page)
        {
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(page);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Screenshot);
        }

        private static async Task<AchievementResult> QueryMoreAchievements(string page)
        {
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(page);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Achievement);
        }

        private static async Task<ScreenshotResult> QueryScreenshots(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Screenshot);
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Screenshot);
        }

        private static async Task<AchievementResult> QueryAchievements(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.Achievement);
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Achievement);
        }

        private static async Task<GameFallback> QueryFallback(string gamename, string rawgkey)
        {
            string RawgRequestUrl = CreateQueryUrl(gamename, rawgkey, QueryType.GameFallback);
            string JsonResponse = "";
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
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeJsonToObject(JsonResponse, QueryType.Game);
        }

        private static string CreateQueryUrl(string gamename, string rawgkey, QueryType type)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();
            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }
            string reqUrl = RawgApiBaseUrl + GameName;
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
                else
                {
                    return JsonConvert.DeserializeObject<ScreenshotResult>(json);
                }
            }
            return null;
        }
    }
}