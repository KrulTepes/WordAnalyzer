using System;
using System.Linq;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Presentators.Console;

namespace WordAnalyzer.Views.Console
{
    public class ColorView : IDataConsoleView
    {
        protected readonly TextType _textType;
        protected readonly string _formatTextTemplate = "{0} - {1}\n";

        public ColorView(TextType textType)
        {
            _textType = textType;
        }

        public ConsolePresentatorModel View(StatisticsModel statistics)
        {
            return new ConsolePresentatorModel
            {
                Items = statistics.Items.Select(x => new ConsolePresentatorItem
                {
                    Text = string.Format(_formatTextTemplate, x.Word, x.Count),
                    Type = _textType
                })
            };
        }
    }
}
