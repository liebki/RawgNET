using RawgNET.Models;

namespace RawgNET
{
    public class RawgClient : IDisposable
    {
        private ClientOptions options;

        public RawgClient(ClientOptions clientOptions)
        {
            options = clientOptions;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void ClientOptionCheck(string gamename)
        {
            if (object.Equals(gamename, null) || gamename.Length < 1)
            {
                NullReferenceException ErrorGame = new("The name of the game is empty or in the wrong format!");
                throw ErrorGame;
            }
            ClientOptionCheckKey();
        }

        private void ClientOptionCheckKey()
        {
            if (object.Equals(options.APIKEY, null) || options.APIKEY.Length < 30 || options.APIKEY.Contains(' '))
            {
                NullReferenceException ErrorKey = new("ApiKey is empty or in the wrong format!");
                throw ErrorKey;
            }
        }

        /// <summary>
        /// Method to query if a game exists
        /// </summary>
        /// <param name="gamename">Name of the game you'd like to query</param>
        /// <returns>A boolean, if the game exists or not</returns>
        public async Task<bool> IsGameExisting(string gamename)
        {
            ClientOptionCheck(gamename);
            return await RawgManager.RawgRequestGameExists(gamename, options.APIKEY);
        }

        /// <summary>
        /// Method to query a game
        /// </summary>
        /// <param name="gamename">Name of the game we'd like to query</param>
        /// <param name="getAchievements">If we want to query for the achievements (takes a second longer)</param>
        /// <returns>Returns a game object</returns>
        public async Task<Game> GetGame(string gamename, bool getAchievements = false, bool getScreenshots = false)
        {
            ClientOptionCheck(gamename);
            return await RawgManager.RawgRequest(gamename, options.APIKEY, getAchievements, getScreenshots);
        }

        /// <summary>
        /// Method to get all known creators, their games, ratings etc.
        /// </summary>
        /// <returns>Returns a list of creators</returns>
        public async Task<List<Creator>> GetCreators(int maxresults = 100)
        {
            ClientOptionCheckKey();
            return await RawgManager.GetAllCreators(options.APIKEY, maxresults);
        }

        public async Task<Creator> GetCreator(string creatorid)
        {
            ClientOptionCheckKey();
            return await RawgManager.GetCreator(options.APIKEY, creatorid);
        }

        public async Task<bool> IsCreatorExisting(string creatorid)
        {
            ClientOptionCheckKey();
            return await RawgManager.IsCreatorExisting(options.APIKEY, creatorid);
        }
    }
}