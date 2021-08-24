using System.Collections;
using System.IO;

namespace WordAnalyzer.Analyzer.InputData
{
    public interface IInputData : IEnumerable
    {
        Stream GetStream();
    }
}
