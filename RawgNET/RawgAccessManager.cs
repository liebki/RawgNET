using Newtonsoft.Json;

namespace RawgNET
{
    internal class RawgAccessManager
    {
        private const string RawgApiBaseUrl = "https://rawg.io/api/games/";

        internal static Game RawgRequest(string name, string rawgkey)
        {
            Game? GameReturnValue = null;
            Task<Game> QueryTryGame = Task.Run(() => QueryGame(name, rawgkey));
            QueryTryGame.Wait();
            if (object.Equals(QueryTryGame.Result.BackgroundImage, null))
            {
                Task<GameFallback> FallbackQueryTry = Task.Run(() => QueryFallback(name, rawgkey));
                FallbackQueryTry.Wait();
                if (FallbackQueryTry.Result.Redirect)
                {
                    Task<Game> SecondQueryTryGame = Task.Run(() => QueryGame(FallbackQueryTry.Result.Slug, rawgkey));
                    QueryTryGame.Wait();
                    QueryTryGame = SecondQueryTryGame;
                }
            }
            if (!object.Equals(QueryTryGame.Result.BackgroundImage, null))
            {
                GameReturnValue = QueryTryGame.Result;
                if (object.Equals(GameReturnValue.Metacritic, null))
                {
                    GameReturnValue.Metacritic = 0;
                }
            }
            return GameReturnValue;
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

        private static string GameNameToQueryUrl(string gamename, string rawgkey)
        {
            string GameName = gamename;
            GameName = GameName.ToLower();
            if (gamename.Contains(' '))
            {
                GameName = GameName.Replace(" ", "-");
            }
            string reqUrl = RawgApiBaseUrl + GameName + "?search_precise=false&search_exact=false&key=" + rawgkey;
            return reqUrl;
        }

        private static GameFallback? DeserializeGameFallbackJson(string json)
        {
            if (!object.Equals(json, null))
            {
                return JsonConvert.DeserializeObject<GameFallback>(json);
            }
            return null;
        }

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