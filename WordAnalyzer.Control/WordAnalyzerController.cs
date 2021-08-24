using System;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Analyzer.InputData;
using WordAnalyzer.Common;
using WordAnalyzer.Repository;

namespace WordAnalyzer.Control
{
    public class WordAnalyzerController
    {
        private readonly IDataAnalyzer _dataAnalyzer;
        private readonly IStatisticsRepository _statisticsRepository;

        public WordAnalyzerController(
            IDataAnalyzerFactory dataAnalyzerFactory,
            IStatisticsRepository statisticsRepository)
        {
            _dataAnalyzer = dataAnalyzerFactory.CreateDataAnalyzer();
            _statisticsRepository = statisticsRepository;
        }

        public StatisticsModel? Analyze(IInputData inputData)
        {
            var statistics = _dataAnalyzer.Analyze(inputData);
            if (statistics == null)
            {
                return null;
            }

            _statisticsRepository.AddAsync(new DbStatistics
            {
                StatisticsId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                JsonData = statistics.Items.ToJson()
            });

            return statistics;
        }
    }
}
