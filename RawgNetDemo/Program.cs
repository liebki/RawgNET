using RawgNET;
using RawgNET.Models;

namespace RawgNetDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (RawgClient client = new(new ClientOptions("c535ca8bfe954571a55df5c7a5a64394")))
            {
                const string query = "gtav";
                Console.WriteLine($"Querying for: {query}");

                if (await client.IsGameExisting(query))
                {
                    Game game = await client.GetGame(query, false, false);
                    Console.WriteLine($"Output for: {game.Name} | {game.NameOriginal} {Environment.NewLine} {game.Description} {Environment.NewLine}");
                }
                else
                {
                    Console.WriteLine("Game does not exist");
                }

                string SomeExistingCreatorsId = "2612";
                Creator cr = await client.GetCreator(SomeExistingCreatorsId);

                Console.WriteLine($"The creator with the id {SomeExistingCreatorsId}");
                Console.WriteLine(cr.ToString());
            }
        }
    }
}