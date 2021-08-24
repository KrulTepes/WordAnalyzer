using Microsoft.Extensions.Logging;
using WordAnalyzer.Analyzer.Parsers;

namespace WordAnalyzer.Analyzer
{
    public class HtmlDataAnalyzerFactory : IDataAnalyzerFactory
    {
        private readonly ILogger<DataAnalyzer> _loggerDataAnalyzer;

        public HtmlDataAnalyzerFactory(ILogger<DataAnalyzer> logger)
        {
            _loggerDataAnalyzer = logger;
        }

        public IDataAnalyzer CreateDataAnalyzer()
        {
            HtmlDataParser htmlDataParser = new();
            DataAnalyzer dataAnalyzer = new(htmlDataParser, _loggerDataAnalyzer);

            return dataAnalyzer;
        }
    }
}
