# RawgNET
A wrapper for the API of wrag.io, to get a game or creator including the data.

## Technologies

### Created using
- .NET Core 6.0

### Nugets/Dependencies used
- Newtonsoft.Json

### Some projects (I know of) using it:
- https://supergames.cf (made by https://github.com/sgamesdev)

## Features

### Nuget
- https://www.nuget.org/packages/RawgNET

### General
- Through methods, like GetGame and IsGameExisting you can check if a game exists or get the data of it.
- Through methods, like GetCreators,GetCreator and IsCreatorExisting you can check if a creator exists or get the data of them.

## Usage

## Example

```
using (RawgClient client = new(new ClientOptions("YOUR KEY FROM https://rawg.io/login?forward=developer")))
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
```

## FAQ

#### Does this work on every OS?

I created this on windows 10 and tested it on other windows 10 machines, I cant guarantee anything for other operating systems or versions. But it should work everywhere, where Core 6 works.

#### Where do I get an API-Key?

At https://rawg.io/apidocs just press the "Get API Key" button.

#### What do I have to be aware of?

Rawg.io has terms of use, please read them and be sure to comply to them - https://api.rawg.io/docs/

## License

**Software:** RawgNET

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

- Get everything from the api inside RawgNet (WIP)
- Clean up the messy RawgAccessManager!!
- More to come..