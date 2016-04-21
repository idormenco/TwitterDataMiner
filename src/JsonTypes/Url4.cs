using System.Collections.Generic;

namespace TwitterDataMiner.JsonTypes
{
    public class Url4
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public List<int> indices { get; set; }
    }
}