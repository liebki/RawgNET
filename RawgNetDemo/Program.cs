using RawgNET.Manager;
using RawgNET.Models;

namespace RawgNetDemo
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            RawgClient client = new(new ClientOptions("YOUR KEY FROM https://rawg.io/apidocs"));
            const string query = "overwatch";

            if (await client.IsGameExisting(query))
            {
                Console.WriteLine($"Querying the input '{query}'");

                // Fetch detailed information about the game
                Game game = await client.GetGame(query, true, true);

                Console.WriteLine($"Game Name: {game.NameOriginal}");
                Console.WriteLine($"Rating: {game.Rating}");
                Console.WriteLine($"Background Image: {game.BackgroundImage}");
                Console.WriteLine($"Metacritic Score: {game.Metacritic}");
                Console.WriteLine($"Release Date: {game.Released}");
                Console.WriteLine($"Platforms: {string.Join(", ", game.Platforms.Select(p => p.Platform.Name))}");

                if (game.AreScreenshotsAvailable)
                {
                    Console.WriteLine($"First Screenshot: {game.Screenshots.First().Image}");
                }

                if (game.AreAchievementsAvailable)
                {
                    Console.WriteLine($"First Achievement: {game.Achievements.First().Name}");
                }
            }
            else
            {
                Console.WriteLine("Game does not exist!");
            }

            Console.WriteLine();

            string someExistingCreatorsId = "333";
            if (await client.IsCreatorExisting(someExistingCreatorsId))
            {
                Creator creator = await client.GetCreator(someExistingCreatorsId);

                Console.WriteLine($"Creator with ID {someExistingCreatorsId}");
                Console.WriteLine($"Name: {creator.Name}");
                Console.WriteLine($"Image URL: {creator.Image}");
                Console.WriteLine($"Background Image URL: {creator.ImageBackground}");
                Console.WriteLine($"Number of Games: {creator.GamesCount}");
            }
            else
            {
                Console.WriteLine($"Creator with ID {someExistingCreatorsId} does not exist!");
            }
        }
    }
}