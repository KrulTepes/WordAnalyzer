using Moq;
using System.Collections.Generic;
using System.Linq;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Analyzer.InputData;
using WordAnalyzer.Analyzer.Parsers;
using Xunit;

namespace WordAnalyzerTests
{
    public class HtmlDataParserTest
    {
        //[Fact]
        //public void GetChars_Test()
        //{
        //    // Arrange
        //    string arg = "<html>I'm&nbsp;a&nbsp;<b>test data</b>!</html>";
        //    string expected = "I'm a test data!";

        //    Mock<IInputData<char>> mock = new();
        //    mock.Setup(d => d.GetEnumerator()).Returns(arg.GetEnumerator());
        //    HtmlDataParser htmlDataParser = new();

        //    // Act
        //    var result = htmlDataParser.GetChars(mock.Object, true);
        //    string text = string.Empty;
        //    foreach (var item in result)
        //    {
        //        text += item;
        //    }

        //    // Assert
        //    Assert.Equal(expected, text);
        //}


        [Fact]
        public void Parse_Test()
        {
            // Arrange
            string arg = "<html>I&rsquo;m this-word is - 'comment string', 'how' you? 351 62 - 633' 64 '45'</html>";
            List<string> expected = new() 
            {
                "I'm",
                "this-word",
                "is",
                "comment",
                "string",
                "how",
                "you"
            };

            Mock<IInputData> mock = new();
            mock.Setup(d => d.GetEnumerator()).Returns(arg.GetEnumerator());
            HtmlDataParser htmlDataParser = new();

            // Act
            var result = htmlDataParser.Parse(mock.Object);
            List<string> actual = new();
            foreach (var item in result)
            {
                actual.Add(item);
            }

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
