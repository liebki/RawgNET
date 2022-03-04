
# RawgNET
An API-Wrapper to get a game including all it's data from https://rawg.io

## Technologies

### Created using
- .NET Core 6.0

### Nuget(s)
- Newtonsoft.Json (Needs to be added to your project too, until I figure out why)

## Features

### General
- Get a "Game" object including the complete data like images, description and more..

## Usage

## Example

```
using (RawgClient client = new(new RawgClientOptions("API KEY OF RAWG.IO")))
{
    Game game = client.GetGameData("NAME OF GAME");
    if (!object.Equals(game, null))
    {
        Console.WriteLine("Imagelink: " + game.BackgroundImage);
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

- Speed up code
- Make code more clean
- Export "Newtonsoft.Json" nuget package with library
- More to come..