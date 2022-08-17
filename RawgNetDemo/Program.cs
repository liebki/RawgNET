using RawgNET;

namespace RawgNetDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (RawgClient client = new(new RawgClientOptions("YOUR KEY FROM https://rawg.io/login?forward=developer")))
            {
                string query = "gtav";
                Console.WriteLine($"Querying for: {query}");

                if (client.IsGameExisting(query))
                {
                    Game game = client.GetGameData(query, true, true);
                    if (!object.Equals(game, null))
                    {
                        Console.WriteLine($"Output for: {game.Name} | {game.NameOriginal}\n");
                    }
                    Console.WriteLine($"Achievements {Environment.NewLine}--------------");
                    foreach (Achievement item in game.Achievements)
                    {
                        Console.WriteLine($"------ {Environment.NewLine} Name: {item.Name} {Environment.NewLine} Description: {item.Description} {Environment.NewLine} Image: {item.Image} {Environment.NewLine}");
                    }
                    Console.WriteLine($"Screenshots {Environment.NewLine}--------------");
                    foreach (Screenshot item in game.Screenshots)
                    {
                        Console.WriteLine($"------ {Environment.NewLine} Id: {item.Id} {Environment.NewLine} Url: {item.Image} {Environment.NewLine} Image: {item.Image} {Environment.NewLine}");
                    }
                }
                else
                {
                    Console.WriteLine("Game does not exist");
                }
            }
        }
    }
}