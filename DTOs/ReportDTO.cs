using System;

namespace Dnn.WebAnalytics
{
    [Serializable]
    public class ReportDTO
    {
        // initialization
        public ReportDTO()
        {
        }

        // public properties
        public string field { get; set; }
        public int count { get; set; }
        public int total { get; set; }
        public int percent { get; set; }
    }

}