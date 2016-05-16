using System;
using System.Collections.Generic;

namespace TwitterDataMiner.DTO
{
    public class TwitterMinerQuery : ITwitterMinerQuery
    {
        private long? _sinceId;
        private long? _maxId;
        const string since_id_str = "&since_id=";
        const string max_id_str = "&max_id=";
        const string count_str = "&count=100";
        const string lang_str = "&lang=";
        public string TwitterQuery { get; set; }
        public string Language { get; set; }

        public string Build()
        {
            return TwitterQuery;
        }

        public ITwitterMinerQuery WithSinceId(long sinceId)
        {
            _sinceId = sinceId;
            return this;
        }

        public ITwitterMinerQuery WithMaxId(long maxId)
        {
            _maxId = maxId;
            return this;
        }

        public ITwitterMinerQuery WithAllOfThisWords(IEnumerable<string> words)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithExactPhrase(string phrase)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithAnyOfThisWords(IEnumerable<string> words)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithNoneOfTheseWords(IEnumerable<string> words)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithTheseHashtags(IEnumerable<string> hashtags)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithLanguageWrittenIn(LanguageEnum language)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithFromTheseAccounts(IEnumerable<string> accounts)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithToTheseAccounts(IEnumerable<string> accounts)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithMentioningTheseAccounts(IEnumerable<string> accounts)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithNearThisPlace(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithFromThisDate(DateTime @from)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithToThisDate(DateTime to)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithDateBetween(DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithAttitude(AttitudeType attitude)
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithRetweetsIncluded()
        {
            throw new NotImplementedException();
        }

        public ITwitterMinerQuery WithFilter(FilterType filter)
        {
            throw new NotImplementedException();
        }
    }
}