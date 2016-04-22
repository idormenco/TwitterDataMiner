namespace TwitterDataMiner.DTO
{
    public interface ITwitterMinerQuery
    {
        string TwitterQuery { get; set; }
        string Build();
        string BuildWithSinceId(long sinceId);
        string BuildWithMaxId(long maxId);
        string BuildWithMaxIdAndSinceId(long maxId, long sinceId);
    }
}