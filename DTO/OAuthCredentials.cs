namespace TwitterDataMiner.DTO
{
    public class OAuthCredentials : IOAuthCredentials
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}