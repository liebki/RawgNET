# RawgNET
A wrapper for the API of wrag.io, to get a game including all it's data

## Technologies

### Created using
- .NET Core 6.0

### Nuget(s)
- Newtonsoft.Json (Needs to be added to your project too, until I figure out why)

## Features

### New
- Thanks to sgamesdev, I got reminded that the screenshots are missing too, those are included by now

### General
- Get a "Game" object including the complete data like images, description, achievements, screenshots and many more things..

## Usage

## Example

```
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
```

## FAQ

#### Does this work on every OS?

I created this on windows 10 and tested it on other windows 10 machines, I cant guarantee anything for other operating systems or versions. But it should work everywhere, where Core 6 works.

#### Where do I get an API-Key?

At https://rawg.io/apidocs just press the "Get API Key" button.

## License

**Software:** RawgNET

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

- Make code more clean especially the ""RawgRequest"" method!
- Clean up the messy RawgAccessManager
- Export "Newtonsoft.Json" nuget package with library
- More to come..
