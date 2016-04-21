namespace TwitterDataMiner.DTO
{
    public class TwitterAuthentificationResponse : ITwitterAuthentificationResponse
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
    }
}