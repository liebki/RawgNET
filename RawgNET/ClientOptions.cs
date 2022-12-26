namespace RawgNET
{
    public class ClientOptions
    {
        public ClientOptions(string? apikey)
        {
            APIKEY = apikey;
        }

        public string? APIKEY { get; set; }
    }
}