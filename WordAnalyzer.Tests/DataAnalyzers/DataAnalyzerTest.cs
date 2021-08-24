using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Analyzer.InputData;
using WordAnalyzer.Analyzer.Parsers;
using Xunit;

namespace WordAnalyzerTests.DataAnalyzers
{
    public class DataAnalyzerTest
    {
        private class StatisticsItemComparer : IEqualityComparer<StatisticsItem>
        {
            public bool Equals(StatisticsItem x, StatisticsItem y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null && y != null || x != null && y == null)
                {
                    return false;
                }

                return x.Word == y.Word && x.Count == y.Count;
            }

            public int GetHashCode([DisallowNull] StatisticsItem obj)
            {
                return obj.Count * obj.Word.Length + obj.Word.Select(x => (int)x).Sum();
            }
        }

        [Fact]
        public void Analazy_Test()
        {
            // Arrange
            StatisticsItemComparer comparer = new StatisticsItemComparer();

            var arg = "Раз два три три три Раз два четыре раз шесть шесть";
            StatisticsModel expected = new()
            {
                Items = new List<StatisticsItem>
                {
                    new ()
                    {
                        Word  = "РАЗ",
                        Count = 3
                    },
                    new ()
                    {
                        Word  = "ДВА",
                        Count = 2
                    },
                    new ()
                    {
                        Word  = "ТРИ",
                        Count = 3
                    },
                    new ()
                    {
                        Word  = "ЧЕТЫРЕ",
                        Count = 1
                    },
                    new ()
                    {
                        Word  = "ШЕСТЬ",
                        Count = 2
                    },
                }
            };

            Mock<ILogger<DataAnalyzer>> mockLogger = new();
            Mock<IInputData> mockInput = new();
            mockInput.Setup(d => d.GetEnumerator()).Returns(arg.GetEnumerator());
            Mock<IDataParser> mockParser = new();
            mockParser.Setup(d => d.Parse(mockInput.Object)).Returns(arg.Split(' '));
            DataAnalyzer dataAnalyzer = new(mockParser.Object, mockLogger.Object);

            // Act
            var result = dataAnalyzer.Analyze(mockInput.Object);

            // Assert
            Assert.Equal(expected.Items, result.Items, comparer);
        }
    }
}
