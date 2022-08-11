namespace RawgNET
{
    public class RawgClient : IDisposable
    {
        private RawgClientOptions options;

        public RawgClient(RawgClientOptions clientOptions)
        {
            options = clientOptions;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The publicy available method that is called
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <param name="getAchievements">If we want to query for the achievements (takes a second longer)</param>
        /// <returns></returns>
        public Game GetGameData(string gamename, bool getAchievements)
        {
            Game game;
            if (!object.Equals(options.APIKEY, null) && options.APIKEY.Length > 10)
            {
                if (!object.Equals(gamename, null) && gamename.Length > 1)
                {
                    game = RawgAccessManager.RawgRequest(gamename, options.APIKEY, getAchievements);
                }
                else
                {
                    NullReferenceException nullReferenceException = new("The name of the game is empty or in the wrong format!");
                    throw nullReferenceException;
                }
            }
            else
            {
                NullReferenceException nullReferenceException = new("ApiKey is empty or in the wrong format!");
                throw nullReferenceException;
            }
            return game;
        }
    }
}