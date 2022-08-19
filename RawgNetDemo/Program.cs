using RawgNET;

namespace RawgNetDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (RawgClient client = new(new RawgClientOptions("YOUR KEY FROM https://rawg.io/login?forward=developer")))
            {
                const string query = "minecraft";
                Console.WriteLine($"Querying for: {query}");

                if (client.IsGameExisting(query))
                {
                    Game game = client.GetGameData(query, true, true);
                    Console.WriteLine($"Output for: {game.Name} | {game.NameOriginal} {Environment.NewLine} {game.Description} {Environment.NewLine}");

                    Console.WriteLine($"Achievements {Environment.NewLine}--------------");
                    foreach (Achievement item in game.Achievements)
                    {
                        Console.WriteLine($"--- {Environment.NewLine} Name: {item.Name} {Environment.NewLine} Description: {item.Description}");
                    }

                    Console.WriteLine($"Screenshots {Environment.NewLine}--------------");
                    foreach (Screenshot item in game.Screenshots)
                    {
                        Console.WriteLine($"--- {Environment.NewLine} Id: {item.Id} {Environment.NewLine} Url: {item.Image}");
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