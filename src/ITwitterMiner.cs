using System.Collections.Generic;
using TwitterDataMiner;
using TwitterDataMiner.DTO;
using TwitterDataMiner.JsonTypes;

namespace TwitterDataMiner
{
    public interface ITwitterMiner
    {
        ITwitterAuthentificationResponse Authentificate(string apiKey, string apiSecret);
        ITwitterAuthentificationResponse Authentificate(IOAuthCredentials oAuthCredentials);
        string SearchByQuery(ITwitterAuthentificationResponse oAuthCredentials, string query);
        RootObject SearchByQueryDeserialized(ITwitterAuthentificationResponse oAuthCredentials, string query);
        IList<string> MineTweets(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null);
        IList<RootObject> MineTweetsDeserialized(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null);
    }
}