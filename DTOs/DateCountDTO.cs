using System;

namespace Dnn.WebAnalytics
{
    [Serializable]
    public class DateCountDTO
    {
        // initialization
        public DateCountDTO()
        {
        }

        // public properties
        public DateTime date { get; set; }
        public int count { get; set; }
    }

}