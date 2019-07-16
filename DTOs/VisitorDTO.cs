using System;

namespace Dnn.WebAnalytics
{
    [Serializable]
    public class VisitorDTO
    {
        // initialization
        public VisitorDTO()
        {
        }

        // public properties
        public int id { get; set; }

        public int portal_id { get; set; }

        public Nullable<int> user_id { get; set; }

        public DateTime created_on_date { get; set; }

        public string user_username { get; set; }
        public string user_displayname { get; set; }
    }

}