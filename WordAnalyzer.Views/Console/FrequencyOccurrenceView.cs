using System;
using System.Linq;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Presentators.Console;

namespace WordAnalyzer.Views.Console
{
    public class FrequencyOccurrenceView : ColorView, IDataConsoleView
    {
        protected readonly OrderBy _orderBy;

        public FrequencyOccurrenceView(TextType textType, OrderBy orderBy) 
            : base(textType)
        {
            _orderBy = orderBy;
        }

        public new ConsolePresentatorModel View(StatisticsModel statistics)
        {
            var orderedItems = _orderBy switch
            {
                OrderBy.ASC  => statistics.Items.OrderBy(x => x.Count),
                OrderBy.DESC => statistics.Items.OrderByDescending(x => x.Count),
                _ => throw new NotImplementedException(),
            };

            return new ConsolePresentatorModel
            {
                Items = orderedItems.Select(x => new ConsolePresentatorItem
                {
                    Text = string.Format(_formatTextTemplate, x.Word, x.Count),
                    Type = _textType
                })
            };
        }
    }
}
