namespace TwitterDataMiner.DTO
{
    public interface ITwitterAuthentificationResponse
    {
        string TokenType { get; }
        string AccessToken { get; }
    }
}