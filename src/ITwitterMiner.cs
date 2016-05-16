using System;
using System.Collections.Generic;
using TwitterDataMiner.DTO;

namespace TwitterDataMiner
{
    public interface ITwitterMiner<T>
    {
        ITwitterAuthentificationResponse Authentificate(string apiKey, string apiSecret);
        ITwitterAuthentificationResponse Authentificate(IOAuthCredentials oAuthCredentials);
        string SearchByQuery(ITwitterAuthentificationResponse oAuthCredentials, string query);
        T SearchByQueryDeserialized(ITwitterAuthentificationResponse oAuthCredentials, string query);
        IList<string> MineTweets(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? sinceId = null);
        void MineTweetsWithProcessor(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? sinceId = null);
        void MineTweetsWithProcessorAsync(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? sinceId = null);
        IList<T> MineTweetsDeserialized(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? sinceId = null);
    }
}