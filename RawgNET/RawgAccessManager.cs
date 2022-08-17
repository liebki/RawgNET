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
        internal static Game RawgRequest(string name, string rawgkey, bool getAchievements)
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
                    Task<GameAchievement> gameAchievementQuery = Task.Run(() => QueryAchievements(GameReturnValue.Slug, rawgkey));
                    GameQueryResult.Wait();

                    if (gameAchievementQuery.Result.Count > 0)
                    {
                        List<Result> AchievementList = new();
                        GameReturnValue.AchievementsAvailable = true;
                        AchievementList.AddRange(gameAchievementQuery.Result.Results);
                        if (gameAchievementQuery.Result.Next?.AbsoluteUri.Contains("page=") == true)
                        {
                            AchievementList.AddRange(QueryAllAchievements(gameAchievementQuery, new()));
                        }
                        GameReturnValue.Achievements = AchievementList;
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
        /// Recursive method, to get every achievement of a game
        /// </summary>
        /// <param name="gameAchievementQuery">The previous Achievement-Object</param>
        /// <param name="AchievementList">The previous Achievement-List</param>
        /// <returns></returns>
        private static List<Result> QueryAllAchievements(Task<GameAchievement> gameAchievementQuery, List<Result> AchievementList)
        {
            Task<GameAchievement> achievementQuery = Task.Run(() => QueryMoreAchievements(gameAchievementQuery.Result.Next.AbsoluteUri));
            achievementQuery.Wait();
            List<Result> AllAchievements = AchievementList;
            AllAchievements.AddRange(achievementQuery.Result.Results);
            if (achievementQuery.Result.Next?.AbsoluteUri.Contains("page=") == true)
            {
                QueryAllAchievements(achievementQuery, AllAchievements);
            }
            return AllAchievements;
        }

        private static async Task<GameAchievement> QueryMoreAchievements(string page)
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

        private static async Task<GameAchievement> QueryAchievements(string gamename, string rawgkey)
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
        /// This method gives us a GameAchievement-Object from a Json-String
        /// </summary>
        /// <param name="json">The Json-String</param>
        /// <returns></returns>
        private static GameAchievement? DeserializeGameAchievementJson(string json)
        {
            if (!object.Equals(json, null))
            {
                return JsonConvert.DeserializeObject<GameAchievement>(json);
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