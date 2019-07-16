using DotNetNuke.Entities.Tabs;
using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Web.Caching;

namespace Dnn.WebAnalytics
{
    [TableName("Community_Visits")]
    [PrimaryKey("id", AutoIncrement = true)]
    [Cacheable("Community_Visits", CacheItemPriority.Default, 20)]
    [Scope("tab_id")]
    public class VisitInfo
    {
        // initialization
        public VisitInfo()
        {
        }

        // public properties
        public Int64 id { get; set; }
        public DateTime date { get; set; }
        public int visitor_id { get; set; }
        public int tab_id { get; set; }
        public string ip { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string language { get; set; }
        public string domain { get; set; }
        public string url { get; set; }
        public string user_agent { get; set; }
        public string device_type { get; set; }
        public string device { get; set; }
        public string platform { get; set; }
        public string browser { get; set; }
        public string referrer_domain { get; set; }
        public string referrer_url { get; set; }
        public string server { get; set; }
        public string activity { get; set; }
        public string campaign { get; set; }
        public Guid? session_id { get; set; }
        public Guid? request_id { get; set; }
        public Guid? last_request_id { get; set; }

        [IgnoreColumn]
        public VisitorInfo Visitor { get; set; }
        [IgnoreColumn]
        public int PortalId { get; set; }
        [IgnoreColumn]
        public TabInfo Tab { get; set; }
    }
}