using System.Collections.Generic;

namespace TwitterDataMiner.JsonTypes
{
    public class RootObject
    {
        public List<Status> statuses { get; set; }
        public SearchMetadata search_metadata { get; set; }
    }
}