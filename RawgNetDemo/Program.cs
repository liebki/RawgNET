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

                Game game = client.GetGameData(query, true);
                if (!object.Equals(game, null))
                {
                    Console.WriteLine($"Output for: {game.Name} | {game.NameOriginal}\n");
                }
                Console.WriteLine($"Achievements {Environment.NewLine}--------------");
                foreach (Result item in game.Achievements)
                {
                    Console.WriteLine($"------ {Environment.NewLine} Name: {item.Name} {Environment.NewLine} Description: {item.Description} {Environment.NewLine} Image: {item.Image} {Environment.NewLine}");
                }
            }
        }
    }
}