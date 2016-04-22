using System.Collections.Generic;

namespace TwitterDataMiner.JsonTypes
{

    public class Hashtag
    {
        public string text { get; set; }
        public List<long> indices { get; set; }
    }
}