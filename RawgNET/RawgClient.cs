using RawgNET.Models;

namespace RawgNET
{
    public class RawgClient
    {
        private ClientOptions Options { get; }

        public RawgClient(ClientOptions clientOptions)
        {
            Options = clientOptions;
        }

        /// <summary>
        /// First method, to check before starting a query: Check that the name is not too short or empty
        /// </summary>
        /// <param name="gamename"></param>
        private void ClientOptionCheck(string gamename)
        {
            if (string.IsNullOrEmpty(gamename) || gamename.Length < 1)
            {
                NullReferenceException ErrorGame = new("The name of the game is empty or in the wrong format!");
                throw ErrorGame;
            }
            ClientOptionCheckKey();
        }

        /// <summary>
        /// Second method, to check before starting a query: Check for some Key
        /// </summary>
        private void ClientOptionCheckKey()
        {
            if (string.IsNullOrEmpty(Options.Key) || Options.Key.Length < 30 || Options.Key.Contains(' '))
            {
                NullReferenceException ErrorKey = new("ApiKey is empty or in the wrong format!");
                throw ErrorKey;
            }
        }

        /// <summary>
        /// Check if the following game exists
        /// </summary>
        /// <param name="gamename">Name of the game</param>
        /// <returns>Boolean, true game exists and false it does not</returns>
        public async Task<bool> IsGameExisting(string gamename)
        {
            ClientOptionCheck(gamename);
            return await RawgManager.RawgRequestGameExists(gamename, Options.Key);
        }

        /// <summary>
        /// Get all the (available) data of a game
        /// </summary>
        /// <param name="gamename">Name of the game</param>
        /// <param name="getAchievements">If we want to query for the achievements (takes a second longer)</param>
        /// <returns>Game object</returns>
        public async Task<Game> GetGame(string gamename, bool getAchievements = false, bool getScreenshots = false)
        {
            ClientOptionCheck(gamename);
            return await RawgManager.RawgRequest(gamename, Options.Key, getAchievements, getScreenshots);
        }

        /// <summary>
        /// Get all (default: 100) creators, including their rating etc.
        /// </summary>
        /// <param name="maxresults">The max. results, to query</param>
        /// <returns>List with creator objects</returns>
        public async Task<List<Creator>> GetCreators(int maxresults = 100)
        {
            ClientOptionCheckKey();
            return await RawgManager.GetAllCreators(Options.Key, maxresults);
        }

        /// <summary>
        /// Get the following creator, including their rating etc.
        /// </summary>
        /// <param name="creatorid">The ID, the creator is known by (on RAWG)</param>
        /// <returns></returns>
        public async Task<Creator> GetCreator(string creatorid)
        {
            ClientOptionCheckKey();
            return await RawgManager.GetCreator(Options.Key, creatorid);
        }

        /// <summary>
        /// Check if the following creator exists
        /// </summary>
        /// <param name="creatorid">The ID, the creator is known by (on RAWG)</param>
        /// <returns>Boolean, true creator exists and false they do not</returns>
        public async Task<bool> IsCreatorExisting(string creatorid)
        {
            ClientOptionCheckKey();
            return await RawgManager.IsCreatorExisting(Options.Key, creatorid);
        }
    }
}