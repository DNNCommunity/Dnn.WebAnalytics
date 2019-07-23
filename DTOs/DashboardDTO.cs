using System;
using System.Collections.Generic;

namespace Dnn.WebAnalytics
{
    public class DashboardDTO
    {
        // initialization
        public DashboardDTO()
        {
        }

        // public properties
        public int view_count { get; set; }
        public int visit_count { get; set; }
        public int visitor_count { get; set; }
        public int user_count { get; set; }

        public List<int> views { get; set; }
        public List<int> visits { get; set; }
        public List<int> visitors { get; set; }
        public List<int> users { get; set; }

    }

}