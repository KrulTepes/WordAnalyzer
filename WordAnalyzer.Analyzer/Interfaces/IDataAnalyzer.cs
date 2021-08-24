using WordAnalyzer.Analyzer.InputData;

namespace WordAnalyzer.Analyzer
{
    public interface IDataAnalyzer
    {
        StatisticsModel? Analyze(IInputData inputData);
    }
}
