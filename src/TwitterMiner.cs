using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitterDataMiner.DTO;

namespace TwitterDataMiner
{
    public class TwitterMiner<T> : ITwitterMiner<T>
    {
        private readonly string TwitterOAuthUrl = "https://api.twitter.com/oauth2/token";
        private readonly string TwitterSearchUrl = "https://api.twitter.com/1.1/search/tweets.json?q=";
        public ITwitterAuthentificationResponse Authentificate(string apiKey, string apiSecret)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ApplicationException("API Key cannot be null or empty!");
            if (string.IsNullOrEmpty(apiSecret)) throw new ApplicationException("API Secret cannot be null or empty!");

            var request = (HttpWebRequest)WebRequest.Create(TwitterOAuthUrl);

            var keySecretConcatenated = string.Concat(apiKey, ":", apiSecret);
            request.Headers.Add("Authorization", GetBase64AuthorizationData(keySecretConcatenated));
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            using (StreamWriter stOut = new StreamWriter(request.GetRequestStream()))
            {
                stOut.Write("grant_type=client_credentials");
            }
            request.Headers.Add("Accept-Encoding", "gzip");
            using (WebResponse oAuthResponse = request.GetResponse())
            {
                using (var reader = new StreamReader(oAuthResponse.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    dynamic twitAuthResponse = JsonConvert.DeserializeObject(result);

                    return new TwitterAuthentificationResponse()
                    {
                        AccessToken = twitAuthResponse.access_token,
                        TokenType = twitAuthResponse.token_type
                    };
                }
            }
        }

        private string GetBase64AuthorizationData(string keySecretConcatenated)
        {
            var plalongextBytes = System.Text.Encoding.UTF8.GetBytes(keySecretConcatenated);
            string base64 = System.Convert.ToBase64String(plalongextBytes);
            return $"Basic {base64}";
        }

        public ITwitterAuthentificationResponse Authentificate(IOAuthCredentials oAuthCredentials)
        {
            if (oAuthCredentials == null) throw new ApplicationException("OAuth credentials are null");
            return Authentificate(oAuthCredentials.ApiKey, oAuthCredentials.ApiSecret);
        }

        public string SearchByQuery(ITwitterAuthentificationResponse oAuthCredentials, string query)
        {
            var json = GetQueryResponse(oAuthCredentials, query);
            return json;
        }

        public T SearchByQueryDeserialized(ITwitterAuthentificationResponse oAuthCredentials, string query)
        {
            var json = GetQueryResponse(oAuthCredentials, query);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public IList<string> MineTweets(ITwitterMinerQuery minerQuery, long numberOfTweets)
        {
            throw new NotImplementedException();
        }

        private string GetQueryResponse(ITwitterAuthentificationResponse oAuthCredentials, string query)
        {
            string json;
            HttpWebRequest apiRequest = (HttpWebRequest)WebRequest.Create(string.Concat(TwitterSearchUrl, query));
            var authorizationData = $"{oAuthCredentials.TokenType} {oAuthCredentials.AccessToken}";
            apiRequest.Headers.Add("Authorization", authorizationData);
            apiRequest.Method = "Get";
            using (WebResponse oAuthResponse = apiRequest.GetResponse())
            {
                using (var reader = new StreamReader(oAuthResponse.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                }
            }
            return json;
        }

        public IList<string> MineTweets(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null)
        {
            long max_id = -1;
            long numberOfTweetsProcessed = 0;
            List<string> minedTwits = new List<string>();
            List<long> lsl = new List<long>();
            while (numberOfTweetsProcessed < numberOfTweets)
            {
                string twitterResult;
                if (max_id <= 0)
                {
                    if (!since_id.HasValue)
                    {
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, minerQuery.Build());
                    }
                    else
                    {
                        var query = minerQuery.BuildWithSinceId(since_id.Value);
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, query);
                    }
                }
                else
                {
                    if (!since_id.HasValue)
                    {
                        var query = minerQuery.BuildWithMaxId(max_id - 1);
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, query);
                    }
                    else
                    {
                        var query = minerQuery.BuildWithMaxIdAndSinceId(max_id - 1, since_id.Value);
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, query);
                    }
                }
                dynamic deserializeObject = JsonConvert.DeserializeObject(twitterResult);
                if (deserializeObject.statuses.Count == 0)
                {
                    return minedTwits;
                }
                numberOfTweetsProcessed += deserializeObject.search_metadata.count.Value;
                max_id = deserializeObject.statuses.Last().id;
                minedTwits.Add(twitterResult);
            }

            return minedTwits;
        }

        public void MineTweetsWithProcessor(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? since_id = null)
        {
            ProcessData(twitterAuthentificationResponse, minerQuery, numberOfTweets, jsonProcessor, since_id, false);
        }
        public void MineTweetsWithProcessorAsync(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? since_id = null)
        {
            ProcessData(twitterAuthentificationResponse, minerQuery, numberOfTweets, jsonProcessor, since_id, true);
        }

        public void MineTweetsWithAsyncProcessor(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, Action<string> jsonProcessor, long? since_id = null)
        {
            ProcessData(twitterAuthentificationResponse, minerQuery, numberOfTweets, jsonProcessor, since_id, true);
        }

        private void ProcessData(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery,
            long numberOfTweets, Action<string> jsonProcessor, long? since_id, bool runAsync)
        {
            long max_id = -1;
            long numberOfTweetsProcessed = 0;
            int numberOfCalls = 0;
            while (numberOfTweetsProcessed < numberOfTweets)
            {
                string twitterResult;
                if (max_id <= 0)
                {
                    if (!since_id.HasValue)
                    {
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, minerQuery.Build());
                    }
                    else
                    {
                        var query = minerQuery.BuildWithSinceId(since_id.Value);
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, query);
                    }
                }
                else
                {
                    if (!since_id.HasValue)
                    {
                        var query = minerQuery.BuildWithMaxId(max_id - 1);
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, query);
                    }
                    else
                    {
                        var query = minerQuery.BuildWithMaxIdAndSinceId(max_id - 1, since_id.Value);
                        twitterResult = SearchByQuery(twitterAuthentificationResponse, query);
                    }
                }
                numberOfCalls++;
                if (numberOfCalls >= 440)
                {
                    Console.WriteLine("going to sleep for 15 minutes");
                    Thread.Sleep(15 * 1000 * 60);
                    numberOfCalls = 0;
                }

                dynamic deserializeObject = JsonConvert.DeserializeObject(twitterResult);
                if (deserializeObject.statuses.Count == 0)
                {
                    break;
                }
                numberOfTweetsProcessed += deserializeObject.search_metadata.count.Value;
                max_id = deserializeObject.statuses.Last.id.Value;
                if (runAsync)
                {
                    Task.Factory.StartNew(() => jsonProcessor(twitterResult));
                }
                else
                {
                    jsonProcessor(twitterResult);
                }
            }
        }

        public IList<T> MineTweetsDeserialized(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null)
        {
            return
                MineTweets(twitterAuthentificationResponse, minerQuery, numberOfTweets)
                    .Select(JsonConvert.DeserializeObject<T>).ToList();
        }
    }
}