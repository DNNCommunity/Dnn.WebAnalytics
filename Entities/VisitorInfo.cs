using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Users;
using System;
using System.Web.Caching;

namespace Dnn.WebAnalytics
{
    [TableName("Community_Visitors")]
    [PrimaryKey("id", AutoIncrement = true)]
    [Cacheable("Community_Visitors", CacheItemPriority.Default, 20)]
    [Scope("portal_id")]
    public class VisitorInfo
    {
        // initialization
        public VisitorInfo()
        {
        }

        // public properties
        public int id { get; set; }
        public int portal_id { get; set; }
        public int? user_id { get; set; }
        public DateTime created_on_date { get; set; }
        public string user_username { get; set; }
        public string user_displayname { get; set; }
        [IgnoreColumn]
        public UserInfo User { get; set; }
    }
}