using Newtonsoft.Json;
using RawgNET.Models;
using System.Net;

namespace RawgNET.Manager
{
    internal class RawgManager
    {
        protected RawgManager()
        { }

        private const string BaseUrl = "https://rawg.io/api";

        #region Creator

        /// <summary>
        /// Retrieves information about a creator from the Rawg API based on the provided creator ID.
        /// </summary>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <param name="creatorid">The ID of the creator to retrieve.</param>
        /// <returns>The creator's information if found, otherwise null.</returns>
        internal static async Task<Creator> GetCreator(string rawgkey, string creatorid)
        {
            string RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, QueryType.Creator, creatorid);
            using HttpClient Client = new();

            HttpResponseMessage response = await Client.GetAsync(RawgRequestUrl);
            string WebResponseAsJson = await response.Content.ReadAsStringAsync();

            Creator creator = DeserializeJsonToObject<Creator>(WebResponseAsJson);
            creator.IsCreatorExisting = creator.Id.HasValue;

            return creator;
        }

        /// <summary>
        /// Checks if a creator with the specified ID exists in the Rawg API.
        /// </summary>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <param name="creatorid">The ID of the creator to check.</param>
        /// <returns>True if the creator exists, false otherwise.</returns>
        internal static async Task<bool> IsCreatorExisting(string rawgkey, string creatorid)
        {
            string RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, QueryType.Creator, creatorid);
            using HttpClient Client = new();

            HttpResponseMessage response = await Client.GetAsync(RawgRequestUrl);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Retrieves a list of creators from the Rawg API.
        /// </summary>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <param name="maxresults">The maximum number of creators to retrieve (optional).</param>
        /// <returns>A collection of creator information.</returns>
        internal static async Task<IEnumerable<Creator>> GetAllCreators(string rawgkey, int maxresults = 0)
        {
            CreatorResult creatorQuery = await Query<CreatorResult>(rawgkey, QueryType.CreatorList);
            List<Creator> creators = new();

            if (creatorQuery.Count > 0)
            {
                int creatorAddCount = 0;

                foreach (Creator cr in creatorQuery.Creators)
                {
                    if (creatorAddCount < maxresults)
                    {
                        creatorAddCount++;
                        creators.Add(cr);
                    }
                }

                while (creatorQuery.Next?.AbsoluteUri.Contains("page=") == true)
                {
                    creatorQuery = await Query<CreatorResult>(rawgkey, QueryType.OtherPage, url: creatorQuery.Next.AbsoluteUri);
                    foreach (Creator cr in creatorQuery.Creators)
                    {
                        if (creatorAddCount < maxresults)
                        {
                            creatorAddCount++;
                            creators.Add(cr);
                        }
                    }
                }
            }

            return creators;
        }

        #endregion Creator

        #region Game

        /// <summary>
        /// Retrieves information about a game from the Rawg API, including achievements and screenshots.
        /// </summary>
        /// <param name="name">The name of the game to search for.</param>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <param name="getAchievements">Flag indicating whether to retrieve achievements for the game.</param>
        /// <param name="getScreenshots">Flag indicating whether to retrieve screenshots for the game.</param>
        /// <returns>The requested game information with optional achievements and screenshots.</returns>
        internal static async Task<Game> RawgRequest(string name, string rawgkey, bool getAchievements, bool getScreenshots)
        {
            Game gameReturnValue = new();
            Game gameQueryResult = await Query<Game>(rawgkey, QueryType.Game, gamename: name);

            if (gameQueryResult.BackgroundImage == null)
            {
                GameFallback gameFallbackQuery = await QueryFallback(name, rawgkey);
                if (gameFallbackQuery.Redirect)
                {
                    gameQueryResult = await Query<Game>(rawgkey, QueryType.Game, gamename: gameFallbackQuery.Slug);
                }
            }

            if (gameQueryResult.BackgroundImage != null)
            {
                gameReturnValue = gameQueryResult;
                gameReturnValue.Metacritic ??= 0;

                if (getAchievements)
                {
                    await GetAndParse<AchievementResult>(rawgkey, gameReturnValue, QueryType.Achievement);
                }

                if (getScreenshots)
                {
                    await GetAndParse<ScreenshotResult>(rawgkey, gameReturnValue, QueryType.Screenshot);
                }
            }

            return gameReturnValue;
        }

        /// <summary>
        /// Retrieves and parses additional data (achievements or screenshots) for a game.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve (AchievementResult or ScreenshotResult).</typeparam>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <param name="gameReturnValue">The game object to update with the parsed data.</param>
        /// <param name="type">The type of data being retrieved (Achievement or Screenshot).</param>
        private static async Task GetAndParse<T>(string rawgkey, Game gameReturnValue, QueryType type)
        {
            dynamic queryResult = await Query<T>(rawgkey, type, gamename: gameReturnValue.Slug);

            if (type == QueryType.Achievement)
            {
                List<Achievement> finalAchievementList = new();

                if (queryResult.Count > 0)
                {
                    finalAchievementList.AddRange(queryResult.Achievements);

                    if (queryResult.Next != null && queryResult.Next.AbsoluteUri.Contains("page="))
                    {
                        List<Achievement> listPart = await QueryEverything<AchievementResult, Achievement>(queryResult, new List<Achievement>());
                        finalAchievementList.AddRange(listPart);
                    }

                    gameReturnValue.AreAchievementsAvailable = true;
                }

                gameReturnValue.Achievements = finalAchievementList;
            }
            else
            {
                List<Screenshot> finalScreenshotList = new();

                if (queryResult.Count > 0)
                {
                    finalScreenshotList.AddRange(queryResult.Screenshots);

                    if (queryResult.Next != null && queryResult.Next.AbsoluteUri.Contains("page="))
                    {
                        List<Screenshot> listPart = await QueryEverything<ScreenshotResult, Screenshot>(queryResult, new List<Screenshot>());
                        finalScreenshotList.AddRange(listPart);
                    }

                    gameReturnValue.AreScreenshotsAvailable = true;
                }

                gameReturnValue.Screenshots = finalScreenshotList;
            }
        }

        /// <summary>
        /// Checks if a game with the specified name exists in the Rawg API.
        /// </summary>
        /// <param name="name">The name of the game to check for.</param>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <returns>True if the game exists, false otherwise.</returns>
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

        /// <summary>
        /// Queries the Rawg API for game information using a fallback approach.
        /// </summary>
        /// <param name="gamename">The name of the game to search for.</param>
        /// <param name="rawgkey">The Rawg API key.</param>
        /// <returns>The game information obtained from the fallback query.</returns>
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

        #region Store

        internal static async Task<Store[]> GetAllGameStores(string rawgkey)
        {
            string RawgRequestUrl = CreateUtilQueryUrls(rawgkey);
            string WebResponseAsJson = string.Empty;

            using (HttpClient Client = new())
            {
                Task<HttpResponseMessage> TaskResponse = Client.GetAsync(RawgRequestUrl);
                TaskResponse.Wait();

                WebResponseAsJson = await TaskResponse.Result.Content.ReadAsStringAsync();
            }

            return DeserializeJsonToObject<StoreResult>(WebResponseAsJson).StoreList;
        }

        #endregion Store

        #region Stuff

        private static async Task<IEnumerable<TItem>> QueryEverything<TResponse, TItem>(TResponse queryResult, List<TItem> itemList) where TResponse : BaseResult<TItem>
        {
            if (queryResult.Next == null)
            {
                return itemList;
            }

            TResponse response = await QueryMorePages<TResponse>(queryResult.Next.AbsoluteUri);
            itemList.AddRange(response.Items);

            if (response.Next != null)
            {
                return await QueryEverything(response, itemList);
            }

            return itemList;
        }

        /// <summary>
        /// Used when more pages or elements need to be queried
        /// </summary>
        /// <param name="page">The next page</param>
        /// <returns>A suitable object, depending on the QueryType input</returns>
        private static async Task<T> QueryMorePages<T>(string page)
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
        /// <typeparam name="T"></typeparam>
        /// <param name="rawgkey"></param>
        /// <param name="type"></param>
        /// <param name="gamename"></param>
        /// <param name="creatorid"></param>
        /// <param name="url"></param>
        /// <returns>A suitable object, depending on the QueryType input</returns>
        private static async Task<T> Query<T>(string rawgkey, QueryType type, string gamename = "", string creatorid = "", string url = "")
        {
            string RawgRequestUrl = string.IsNullOrEmpty(url) ? CreateQueryUrl(gamename, rawgkey, type) : url;
            string WebResponseAsJson;

            if (type == QueryType.Creator || type == QueryType.CreatorList)
            {
                RawgRequestUrl = CreateNonGameQueryUrl(rawgkey, type, creatorid);
            }

            using (HttpClient client = new())
            {
                HttpResponseMessage response = await client.GetAsync(RawgRequestUrl);
                WebResponseAsJson = await response.Content.ReadAsStringAsync();
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

            switch (type)
            {
                case QueryType.CreatorList:
                    reqUrl += "/creators";
                    break;
                case QueryType.Creator:
                    reqUrl += $"/creators/{creatorid}";
                    break;
            }

            return $"{reqUrl}?key={rawgkey}";
        }

        private static string CreateUtilQueryUrls(string rawgkey)
        {
            string reqUrl = $"{BaseUrl}/stores?key={rawgkey}";
            return reqUrl;
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
            string Gamename = gamename.ToLower();

            if (gamename.Contains(' '))
            {
                Gamename = Gamename.Replace(" ", "-");
            }

            string reqUrl = $"{BaseUrl}/games/{Gamename}";
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