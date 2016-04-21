using System.Collections.Generic;

namespace TwitterDataMiner.JsonTypes
{
    public class BoundingBox
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }
}