namespace RawgNET
{
    public class RawgClient : IDisposable
    {
        private RawgClientOptions options;

        public RawgClient(RawgClientOptions clientOptions)
        {
            options = clientOptions;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Game GetGameData(string gamename)
        {
            Game game;
            if (!object.Equals(options.APIKEY, null) && options.APIKEY.Length > 10)
            {
                if (!object.Equals(gamename, null) && gamename.Length > 1)
                {
                    game = RawgAccessManager.RawgRequest(gamename, options.APIKEY);
                }
                else
                {
                    NullReferenceException nullReferenceException = new("The name of the game is empty or in the wrong format!");
                    throw nullReferenceException;
                }
            }
            else
            {
                NullReferenceException nullReferenceException = new("ApiKey is empty or in the wrong format!");
                throw nullReferenceException;
            }
            return game;
        }
    }
}