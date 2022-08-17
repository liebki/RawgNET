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
        /// Method to query if a game exists, where key and name are checked first
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <returns>A boolean, if the game exists or not</returns>
        public bool IsGameExisting(string gamename)
        {
            if (object.Equals(options.APIKEY, null) || options.APIKEY.Length < 30 || options.APIKEY.Contains(' '))
            {
                if (object.Equals(gamename, null) || gamename.Length < 1)
                {
                    NullReferenceException ErrorGame = new("The name of the game is empty or in the wrong format!");
                    throw ErrorGame;
                }

                NullReferenceException ErrorKey = new("ApiKey is empty or in the wrong format!");
                throw ErrorKey;
            }
            return RawgAccessManager.RawgRequestGameExists(gamename, options.APIKEY);
        }

        /// <summary>
        /// Method to query a game, where key and name are checked first
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <param name="getAchievements">If we want to query for the achievements (takes a second longer)</param>
        /// <returns>Returns a game object, which contains the data</returns>
        public Game GetGameData(string gamename, bool getAchievements, bool getScreenshots)
        {
            if (object.Equals(options.APIKEY, null) || options.APIKEY.Length < 30 || options.APIKEY.Contains(' '))
            {
                if (object.Equals(gamename, null) || gamename.Length < 1)
                {
                    NullReferenceException ErrorGame = new("The name of the game is empty or in the wrong format!");
                    throw ErrorGame;
                }

                NullReferenceException ErrorKey = new("ApiKey is empty or in the wrong format!");
                throw ErrorKey;
            }
            return RawgAccessManager.RawgRequest(gamename, options.APIKEY, getAchievements, getScreenshots);
        }
    }
}