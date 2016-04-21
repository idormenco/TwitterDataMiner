using System;
using System.Collections.Generic;
using System.IO;
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
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(keySecretConcatenated);
            string base64 = System.Convert.ToBase64String(plainTextBytes);
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

        private string GetQueryResponse(ITwitterAuthentificationResponse oAuthCredentials, string query)
        {
            string json;
            HttpWebRequest apiRequest = (HttpWebRequest) WebRequest.Create(string.Concat(TwitterSearchUrl, query));
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

        public IList<string> MineTweets(ITwitterMinerQuery minerQuery, int numberOfTweets)
        {
            throw new NotImplementedException();
        }

        public IList<RootObject> MineTweetsDeserialized(ITwitterMinerQuery minerQuery, int numberOfTweets)
        {
            throw new NotImplementedException();
        }
    }
}