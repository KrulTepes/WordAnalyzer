using System.Collections.Generic;

namespace WordAnalyzer.Presentators.Console
{
    public class ConsolePresentatorModel
    {
        public IEnumerable<ConsolePresentatorItem> Items { get; set; } = new List<ConsolePresentatorItem>();
    }
}
