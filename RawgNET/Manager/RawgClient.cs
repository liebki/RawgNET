using RawgNET.Models;

namespace RawgNET.Manager
{
    public class RawgClient
    {
        private readonly ClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the RawgClient class.
        /// </summary>
        /// <param name="clientOptions">The client options including API key.</param>
        public RawgClient(ClientOptions clientOptions)
        {
            if (string.IsNullOrEmpty(clientOptions.Key) || clientOptions.Key.Length < 30 || clientOptions.Key.Contains(' '))
            {
                throw new ArgumentException("Invalid API key format.", nameof(clientOptions.Key));
            }

            _options = clientOptions;
        }

        /// <summary>
        /// Checks if the provided game name is valid.
        /// </summary>
        /// <param name="gamename">The name of the game to check.</param>
        /// <exception cref="ArgumentException">Thrown when the game name is empty or in the wrong format.</exception>
        private void CheckGameName(string gamename)
        {
            if (string.IsNullOrEmpty(gamename) || gamename.Length < 1)
            {
                throw new ArgumentException("Game name is empty or in the wrong format.");
            }
        }

        /// <summary>
        /// Checks if a game with the specified name exists.
        /// </summary>
        /// <param name="gamename">The name of the game to check.</param>
        /// <returns>True if the game exists, false otherwise.</returns>
        public async Task<bool> IsGameExisting(string gamename)
        {
            CheckGameName(gamename);
            return await RawgManager.RawgRequestGameExists(gamename, _options.Key);
        }

        /// <summary>
        /// Gets detailed information about a game.
        /// </summary>
        /// <param name="gamename">The name of the game.</param>
        /// <param name="getAchievements">Whether to include achievements.</param>
        /// <param name="getScreenshots">Whether to include screenshots.</param>
        /// <returns>A Game object representing the game.</returns>
        public async Task<Game> GetGame(string gamename, bool getAchievements = false, bool getScreenshots = false)
        {
            CheckGameName(gamename);
            return await RawgManager.RawgRequest(gamename, _options.Key, getAchievements, getScreenshots);
        }

        /// <summary>
        /// Gets a list of creators.
        /// </summary>
        /// <param name="maxresults">The maximum number of results to retrieve.</param>
        /// <returns>An IEnumerable of Creator objects.</returns>
        public async Task<IEnumerable<Creator>> GetCreators(int maxresults = 100)
        {
            return await RawgManager.GetAllCreators(_options.Key, maxresults);
        }

        /// <summary>
        /// Get all available stores which sell games
        /// </summary>
        public async Task<Store[]> GetAllGameStores()
        {
            return await RawgManager.GetAllGameStores(_options.Key);
        }

        /// <summary>
        /// Gets detailed information about a specific creator.
        /// </summary>
        /// <param name="creatorid">The ID of the creator.</param>
        /// <returns>A Creator object representing the creator.</returns>
        public async Task<Creator> GetCreator(string creatorid)
        {
            return await RawgManager.GetCreator(_options.Key, creatorid);
        }

        /// <summary>
        /// Checks if a creator with the specified ID exists.
        /// </summary>
        /// <param name="creatorid">The ID of the creator.</param>
        /// <returns>True if the creator exists, false otherwise.</returns>
        public async Task<bool> IsCreatorExisting(string creatorid)
        {
            return await RawgManager.IsCreatorExisting(_options.Key, creatorid);
        }
    }
}