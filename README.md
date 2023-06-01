# RawgNET
A wrapper for the API of wrag.io, to get a game or creator including all their data.

## Technologies

### Created using
- .NET Core 6.0

### NuGet/Dependencies used
- Newtonsoft.Json

### Projects/People (I know of) using it:
- https://github.com/sgamesdev)

## Features

### Nuget
- https://www.nuget.org/packages/RawgNET

### General
- Through methods, like ```GetGame()``` and ```IsGameExisting()``` you can check if a game exists or get the data of it.
- Through methods, like ```GetCreators()```, ```GetCreator()``` and ```IsCreatorExisting()``` you can check if a creator exists or get the data of them.

## Usage

## Example

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
