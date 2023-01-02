using RawgNET;
using RawgNET.Models;

namespace RawgNetDemo
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            RawgClient client = new(new ClientOptions("YOUR KEY FROM https://rawg.io/login?forward=developer"));
            const string query = "gtav";

            Console.WriteLine($"Querying the input {query}");

            if (await client.IsGameExisting(query))
            {
                Game game = await client.GetGame(query, true, true);
                Console.WriteLine($"Name: {game.NameOriginal} - Rating: {game.Rating}");

                if (game.ScreenshotsAvailable)
                {
                    Console.WriteLine($"First screenshot: {game.Screenshots.First().Image}");
                }
                if (game.AchievementsAvailable)
                {
                    Console.WriteLine($"First achievement: {game.Achievements.First().Name}");
                }
            }
            else
            {
                Console.WriteLine("Game does not exist!");
            }

            Console.WriteLine();

            string SomeExistingCreatorsId = "444";
            Creator cr = await client.GetCreator(SomeExistingCreatorsId);

            Console.WriteLine($"The creator with id {SomeExistingCreatorsId}");
            Console.WriteLine($"Name: {cr.Name} - Image: {cr.Image}");
        }
    }
}