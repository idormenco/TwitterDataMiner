using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterDataMiner;
using TwitterDataMiner.DTO;

namespace TwitterDataMiner
{
    public interface ITwitterMiner<T>
    {
        ITwitterAuthentificationResponse Authentificate(string apiKey, string apiSecret);
        ITwitterAuthentificationResponse Authentificate(IOAuthCredentials oAuthCredentials);
        string SearchByQuery(ITwitterAuthentificationResponse oAuthCredentials, string query);
        T SearchByQueryDeserialized(ITwitterAuthentificationResponse oAuthCredentials, string query);
        IList<string> MineTweets(ITwitterMinerQuery minerQuery, long numberOfTweets);
        IList<string> MineTweets(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null);
        void MineTweetsWithProcessor(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? since_id = null);
        void MineTweetsWithProcessorAsync(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? since_id = null);
        IList<T> MineTweetsDeserialized(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null);
    }
}