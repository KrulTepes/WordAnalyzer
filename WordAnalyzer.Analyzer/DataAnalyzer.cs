using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using WordAnalyzer.Analyzer.InputData;
using WordAnalyzer.Analyzer.Parsers;

namespace WordAnalyzer.Analyzer
{
    public class DataAnalyzer : IDataAnalyzer
    {
        private readonly IDataParser _parser;
        private readonly ILogger<DataAnalyzer> _logger;

        public DataAnalyzer(
            IDataParser parser,
            ILogger<DataAnalyzer> logger)
        {
            _parser = parser;
            _logger = logger;
        }

        public StatisticsModel? Analyze(IInputData inputData)
        {
            try
            {
                var wordInterator = _parser.Parse(inputData);

                List<StatisticsItem> items = new();

                foreach (var word in wordInterator)
                {
                    var wordNormalize = word.ToUpper();

                    var item = items.FirstOrDefault(x => x.Word == wordNormalize);

                    if (item != null)
                    {
                        item.Count++;
                    }
                    else
                    {
                        items.Add(new StatisticsItem
                        {
                            Word = wordNormalize,
                            Count = 1
                        });
                    }
                }

                return new StatisticsModel
                {
                    Items = items
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Parse input data failed.\nMessage:{e.Message}\nStackTrace: {e.StackTrace}");
                return null;
            }
        }
    }
}
