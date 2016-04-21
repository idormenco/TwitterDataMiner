using System.Collections.Generic;

namespace TwitterDataMiner.JsonTypes
{
    public class Entities
    {
        public List<Hashtag> hashtags { get; set; }
        public List<object> symbols { get; set; }
        public List<object> user_mentions { get; set; }
        public List<object> urls { get; set; }
        public List<Medium> media { get; set; }
    }
}