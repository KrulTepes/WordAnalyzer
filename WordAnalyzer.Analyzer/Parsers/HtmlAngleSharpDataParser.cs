using System;
using System.Collections.Generic;
using AngleSharp.Html.Parser;
using WordAnalyzer.Analyzer.InputData;

namespace WordAnalyzer.Analyzer.Parsers
{
    public class HtmlAngleSharpDataParser : IDataParser
    {
        private readonly HtmlParser _htmlParser;

        public HtmlAngleSharpDataParser()
        {
            _htmlParser = new HtmlParser();
        }
        
        public IEnumerable<string> Parse(IInputData inputData)
        {
            string word = string.Empty;
            
            foreach (var text in _htmlParser.ParseDocumentCustom(inputData.GetStream()))
            {
                foreach (var symbol in text)
                {
                    if (char.IsLetterOrDigit(symbol))
                    {
                        word += symbol;
                    }
                    else if (word != string.Empty)
                    {
                        yield return word;
                        word = String.Empty;
                    }
                }
            }

            yield break;
        }
    }
}
