using Microsoft.Extensions.Logging;
using WordAnalyzer.Analyzer.Parsers;

namespace WordAnalyzer.Analyzer
{
    public class HtmlAngelSharpDataAnalyzerFactory : IDataAnalyzerFactory
    {
        private readonly ILogger<DataAnalyzer> _loggerDataAnalyzer;

        public HtmlAngelSharpDataAnalyzerFactory(ILogger<DataAnalyzer> logger)
        {
            _loggerDataAnalyzer = logger;
        }

        public IDataAnalyzer CreateDataAnalyzer()
        {
            HtmlAngleSharpDataParser htmlDataParser = new();
            DataAnalyzer dataAnalyzer = new(htmlDataParser, _loggerDataAnalyzer);

            return dataAnalyzer;
        }
    }
}
