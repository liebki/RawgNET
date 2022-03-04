namespace RawgNET
{
    public class RawgClientOptions
    {
        public RawgClientOptions(string? aPIKEY)
        {
            APIKEY = aPIKEY;
        }

        public string? APIKEY { get; set; }
    }
}