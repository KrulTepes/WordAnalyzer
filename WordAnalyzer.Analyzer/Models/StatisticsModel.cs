using System.Collections.Generic;

namespace WordAnalyzer.Analyzer
{
    public class StatisticsModel
    {
        public IEnumerable<StatisticsItem> Items { get; set; } = new List<StatisticsItem>();
    }
}
