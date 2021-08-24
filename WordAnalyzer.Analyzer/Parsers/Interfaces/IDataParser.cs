using System.Collections.Generic;
using WordAnalyzer.Analyzer.InputData;

namespace WordAnalyzer.Analyzer.Parsers
{
    public interface IDataParser
    {
        IEnumerable<string> Parse(IInputData inputData);
    }
}
