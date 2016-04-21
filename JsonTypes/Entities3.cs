using System.Collections.Generic;

namespace TwitterDataMiner.JsonTypes
{
    public class Entities3
    {
        public List<Hashtag2> hashtags { get; set; }
        public List<object> symbols { get; set; }
        public List<object> user_mentions { get; set; }
        public List<object> urls { get; set; }
        public List<Medium3> media { get; set; }
    }
}