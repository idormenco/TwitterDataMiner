namespace TwitterDataMiner.DTO
{
    public class TwitterMinerQuery : ITwitterMinerQuery
    {
        const string since_id_str = "&since_id=";
        const string max_id_str = "&max_id=";
        public string TwitterQuery { get; set; }
        public string Build()
        {
            return TwitterQuery;
        }

        public string BuildWithSinceId(long sinceId)
        {
            return string.Concat(TwitterQuery, since_id_str, sinceId);
        }

        public string BuildWithMaxId(long maxId)
        {
            return string.Concat(TwitterQuery, max_id_str, maxId);
        }

        public string BuildWithMaxIdAndSinceId(long maxId, long sinceId)
        {
            return string.Concat(TwitterQuery, max_id_str, maxId, since_id_str, sinceId);
        }
    }
}