# RawgNET
A wrapper for the API of rawg.io, to get a game or creator including all their data.

<img src="https://www.startuplithuania.com/wp-content/uploads/2018/10/Rawg-promo-cover.png" width="800">

## Technologies

### Created using
- .NET Core 6.0

### NuGet/Dependencies used
- Newtonsoft.Json

### Projects/People (I know of) using it:
- [GitHub Repository](https://github.com/sgamesdev)

## Features

### Nuget
- [NuGet Package](https://www.nuget.org/packages/RawgNET)

### General
- Through methods, like `GetGame()` and `IsGameExisting()`, you can check if a game exists or get the data of it.
- Through methods, like `GetCreators()`, `GetCreator()`, and `IsCreatorExisting()`, you can check if a creator exists or get the data of them.

## Usage

## Example (see the "RawgNetDemo" project)

```csharp
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
```

## FAQ

#### Where do I get an API-Key?

At https://rawg.io/apidocs, just press the "Get API Key" button.

#### What do I have to be aware of?

Rawg.io has terms of use, please read them and be sure to comply to them - https://api.rawg.io/docs/

## License

**Software:** RawgNET

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

- Get everything from the API inside RawgNet (WIP)
- Clean up and reduce code
- More to come…
