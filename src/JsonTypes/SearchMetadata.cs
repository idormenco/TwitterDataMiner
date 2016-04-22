namespace TwitterDataMiner.JsonTypes
{
    public class SearchMetadata
    {
        public double completed_in { get; set; }
        public long max_id { get; set; }
        public string max_id_str { get; set; }
        public string next_results { get; set; }
        public string query { get; set; }
        public string refresh_url { get; set; }
        public long count { get; set; }
        public long since_id { get; set; }
        public string since_id_str { get; set; }
    }
}