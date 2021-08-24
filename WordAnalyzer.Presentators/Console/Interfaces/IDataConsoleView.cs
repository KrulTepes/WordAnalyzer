using WordAnalyzer.Analyzer;

namespace WordAnalyzer.Presentators.Console
{
    public interface IDataConsoleView
    {
        ConsolePresentatorModel View(StatisticsModel statistics);
    }
}
