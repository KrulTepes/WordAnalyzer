using System;

namespace WordAnalyzer.Repository
{
    public class DbStatistics
    {
        public Guid StatisticsId { get; set; }
        public DateTime Date { get; set; }
        public string? JsonData { get; set; }
    }
}
