using System;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Control;

namespace WordAnalyzer.Presentators.Console
{
    public class ConsolePresentator : IDataPresentator
    {
        private readonly IDataConsoleView _dataConsoleView;

        public ConsolePresentator(IDataConsoleView dataConsoleView)
        {
            _dataConsoleView = dataConsoleView;
        }

        public void Show(StatisticsModel statistics)
        {
            foreach (var item in _dataConsoleView.View(statistics).Items)
            {
                WriteLine(item);
            }
        }

        public static void WriteLine(ConsolePresentatorItem item)
        {
            ColorItem oldColors = new ColorItem
            {
                Foreground = System.Console.ForegroundColor,
                Background = System.Console.BackgroundColor
            };

            SetConsoleColors(GetColors(item.Type));
            System.Console.Write(item.Text);
            SetConsoleColors(oldColors);
        }

        public static string? ReadLine(TextType type)
        {
            ColorItem oldColors = new ColorItem
            {
                Foreground = System.Console.ForegroundColor,
                Background = System.Console.BackgroundColor
            };

            SetConsoleColors(GetColors(type));
            string? res = System.Console.ReadLine();
            SetConsoleColors(oldColors);

            return res;
        }

        private static ColorItem GetColors(TextType type)
        {
            ColorItem item = new ColorItem 
            { 
                Foreground = ConsoleColor.White, 
                Background = ConsoleColor.Black
            };

            switch (type)
            {
                case TextType.UI:
                    item = new ColorItem
                    {
                        Foreground = ConsoleColor.Yellow,
                        Background = ConsoleColor.Black
                    };
                    break;
                case TextType.Info:
                    item = new ColorItem
                    {
                        Foreground = ConsoleColor.White,
                        Background = ConsoleColor.Black
                    };
                    break;
                case TextType.Error:
                    item = new ColorItem
                    {
                        Foreground = ConsoleColor.Red,
                        Background = ConsoleColor.Black
                    };
                    break;
                case TextType.Control:
                    item = new ColorItem
                    {
                        Foreground = ConsoleColor.Green,
                        Background = ConsoleColor.Black
                    };
                    break;
            }

            return item;
        }

        private static void SetConsoleColors(ColorItem item)
        {
            System.Console.ForegroundColor = item.Foreground;
            System.Console.BackgroundColor = item.Background;
        }
    }
}
