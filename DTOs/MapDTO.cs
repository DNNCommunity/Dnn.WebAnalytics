using System;

namespace Dnn.WebAnalytics
{
    public class MapDTO
    {
        // initialization
        public MapDTO()
        {
        }

        // public properties
        public Nullable<int> user_id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

}