namespace RawgNET
{
    public class RawgClientOptions
    {
        public RawgClientOptions(string? apikey)
        {
            APIKEY = apikey;
        }

        public string? APIKEY { get; set; }
    }
}