using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TwitterDataMiner.DTO;
using TwitterDataMiner.JsonTypes;

namespace TwitterDataMiner
{
    public class TwitterMiner : ITwitterMiner
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

        public RootObject SearchByQueryDeserialized(ITwitterAuthentificationResponse oAuthCredentials, string query)
        {
            var json = GetQueryResponse(oAuthCredentials, query);
            return JsonConvert.DeserializeObject<RootObject>(json);
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
            List<long> lsl= new List<long>();
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
                var deserializeObject = JsonConvert.DeserializeObject<RootObject>(twitterResult);
                if (deserializeObject.statuses.Count == 0)
                {
                    return minedTwits;
                }
                numberOfTweetsProcessed += deserializeObject.search_metadata.count;
                max_id = deserializeObject.statuses.Last().id;
                minedTwits.Add(twitterResult);
            }

            return minedTwits;
        }

        public IList<RootObject> MineTweetsDeserialized(ITwitterAuthentificationResponse twitterAuthentificationResponse, ITwitterMinerQuery minerQuery, long numberOfTweets, long? since_id = null)
        {
            return
                MineTweets(twitterAuthentificationResponse, minerQuery, numberOfTweets)
                    .Select(JsonConvert.DeserializeObject<RootObject>).ToList();
        }
    }
}