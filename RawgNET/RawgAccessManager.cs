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
                    Task<AchievementResult> gameAchievementQuery = Task.Run(() => QueryAchievements(GameReturnValue.Slug, rawgkey));
                    GameQueryResult.Wait();

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

                GameReturnValue.ScreenshotsAvailable = false;
                GameReturnValue.Screenshots = new();

                if (getScreenshots)
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
            return GameReturnValue;
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
            return DeserializeScreenshotJson(JsonResponse);
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
            return DeserializeGameAchievementJson(JsonResponse);
        }

        private static async Task<ScreenshotResult> QueryScreenshots(string gamename, string rawgkey)
        {
            string RawgRequestUrl = GameNameToScreenshotQueryUrl(gamename, rawgkey);
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeScreenshotJson(JsonResponse);
        }

        private static async Task<AchievementResult> QueryAchievements(string gamename, string rawgkey)
        {
            string RawgRequestUrl = GameNameToAchievementQueryUrl(gamename, rawgkey);
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeGameAchievementJson(JsonResponse);
        }

        private static async Task<GameFallback> QueryFallback(string gamename, string rawgkey)
        {
            string RawgRequestUrl = GameNameToQueryUrl(gamename, rawgkey);
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeGameFallbackJson(JsonResponse);
        }

        private static async Task<Game> QueryGame(string gamename, string rawgkey)
        {
            string RawgRequestUrl = GameNameToQueryUrl(gamename, rawgkey);
            string JsonResponse = "";
            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();
                JsonResponse = await TaskResponse.Result.Content.ReadAsStringAsync();
            }
            return DeserializeGameJson(JsonResponse);
        }

        /// <summary>
        /// Method to build a ready-to-query url (screenshots)
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <param name="rawgkey">Your API-Key</param>
        /// <returns></returns>
        private static string GameNameToScreenshotQueryUrl(string gamename, string rawgkey)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();
            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }
            string reqUrl = RawgApiBaseUrl + GameName + $"/screenshots?key={rawgkey}";
            return reqUrl;
        }

        /// <summary>
        /// Method to build a ready-to-query url (achievements)
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <param name="rawgkey">Your API-Key</param>
        /// <returns></returns>
        private static string GameNameToAchievementQueryUrl(string gamename, string rawgkey)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();
            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }
            string reqUrl = RawgApiBaseUrl + GameName + $"/achievements?key={rawgkey}";
            return reqUrl;
        }

        /// <summary>
        /// Method to build a ready-to-query url (games)
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <param name="rawgkey">Your API-Key</param>
        /// <returns></returns>
        private static string GameNameToQueryUrl(string gamename, string rawgkey)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();
            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }
            string reqUrl = RawgApiBaseUrl + GameName + $"?search_precise=false&search_exact=false&key={rawgkey}";
            return reqUrl;
        }

        /// <summary>
        /// This method gives us a ScreenshotResult-Object from a Json-String
        /// </summary>
        /// <param name="json">The Json-String</param>
        /// <returns></returns>
        private static ScreenshotResult? DeserializeScreenshotJson(string json)
        {
            if (!object.Equals(json, null))
            {
                return JsonConvert.DeserializeObject<ScreenshotResult>(json);
            }
            return null;
        }

        /// <summary>
        /// This method gives us a AchievementResult-Object from a Json-String
        /// </summary>
        /// <param name="json">The Json-String</param>
        /// <returns></returns>
        private static AchievementResult? DeserializeGameAchievementJson(string json)
        {
            if (!object.Equals(json, null))
            {
                return JsonConvert.DeserializeObject<AchievementResult>(json);
            }
            return null;
        }

        /// <summary>
        /// This method gives us a GameFallback-Object from a Json-String
        /// </summary>
        /// <param name="json">The Json-String</param>
        /// <returns></returns>
        private static GameFallback? DeserializeGameFallbackJson(string json)
        {
            if (!object.Equals(json, null))
            {
                return JsonConvert.DeserializeObject<GameFallback>(json);
            }
            return null;
        }

        /// <summary>
        /// This method gives us a Game-Object from a Json-String
        /// </summary>
        /// <param name="json">The Json-String</param>
        /// <returns></returns>
        private static Game? DeserializeGameJson(string json)
        {
            if (!object.Equals(json, null))
            {
                return JsonConvert.DeserializeObject<Game>(json);
            }
            return null;
        }
    }
}