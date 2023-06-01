# RawgNET
The wrapper for the API of wrag.io, to get a game or creator including the data.

```
RawgClient client = new(new ClientOptions("YOUR KEY FROM https://rawg.io/apidocs"));
const string query = "gtav";

if (await client.IsGameExisting(query))
{
    Console.WriteLine($"Querying the input {query}");
    Game game = await client.GetGame(query, true, true);

    Console.WriteLine($"Name: {game.NameOriginal} - Rating: {game.Rating} - Image: {game.BackgroundImage}");

    if (game.AreScreenshotsAvailable)
    {
        Console.WriteLine($"First screenshot: {game.Screenshots.First().Image}");
    }
    if (game.AreAchievementsAvailable)
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
```