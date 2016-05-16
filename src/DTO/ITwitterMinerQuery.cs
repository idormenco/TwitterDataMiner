using System;
using System.Collections.Generic;

namespace TwitterDataMiner.DTO
{
    public interface ITwitterMinerQuery
    {
        string Build();
        ITwitterMinerQuery WithSinceId(long sinceId);
        ITwitterMinerQuery WithMaxId(long maxId);
        ITwitterMinerQuery WithAllOfThisWords(IEnumerable<string> words);
        ITwitterMinerQuery WithExactPhrase(string phrase);
        ITwitterMinerQuery WithAnyOfThisWords(IEnumerable<string> words);
        ITwitterMinerQuery WithNoneOfTheseWords(IEnumerable<string> words);
        ITwitterMinerQuery WithTheseHashtags(IEnumerable<string> hashtags);
        ITwitterMinerQuery WithLanguageWrittenIn(LanguageEnum language);
        ITwitterMinerQuery WithFromTheseAccounts(IEnumerable<string> accounts);
        ITwitterMinerQuery WithToTheseAccounts(IEnumerable<string> accounts);
        ITwitterMinerQuery WithMentioningTheseAccounts(IEnumerable<string> accounts);
        ITwitterMinerQuery WithNearThisPlace(Coordinates coordinates);
        ITwitterMinerQuery WithFromThisDate(DateTime from);
        ITwitterMinerQuery WithToThisDate(DateTime to);
        ITwitterMinerQuery WithDateBetween(DateTime from, DateTime to);
        ITwitterMinerQuery WithAttitude(AttitudeType attitude);
        ITwitterMinerQuery WithRetweetsIncluded();
        ITwitterMinerQuery WithFilter(FilterType filter);
    }
}