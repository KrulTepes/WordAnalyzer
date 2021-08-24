using System;
using System.Collections.Generic;
using System.Linq;
using WordAnalyzer.Analyzer.InputData;

namespace WordAnalyzer.Analyzer.Parsers
{
    public class HtmlDataParser : IDataParser
    {
        [Flags]
        private enum AnalyzerFlags
        {
            None = 0,
            WaitBeginText = 1,
            WaitEndText = 2,
            WaitEndSpecSymbol = 4,
        }

        private readonly List<char> separators = new()
        {
            ' ',
            '\n',
            '\r',
            '\t',
            ',',
            '.',
            ';',
            ':',
            '!',
            '?',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '*',
            '(',
            ')',
            '[',
            ']',
            '{',
            '}',
            '+',
            '=',
            '\"',
            '<',
            '>',
            '/',
            '\\',
            '`',
            '~',
            '|',
            '-',
            '\''
        };

        public IEnumerable<string> Parse(IInputData inputData)
        {
            string word = string.Empty;

            char preChar = default;
            foreach (char symbol in GetChars(inputData))
            {
                if (char.IsNumber(symbol))
                {
                    continue;
                }

                switch (symbol)
                {
                    case '\'':
                        if (preChar == ' ')
                        {
                            preChar = symbol;
                            if (word != string.Empty)
                            {
                                yield return word;
                                word = string.Empty;
                            }
                            continue;
                        }
                        else
                        {
                            preChar = symbol;
                            word += symbol;
                            continue;
                        }
                    case '-':
                        if (char.IsPunctuation(preChar) || char.IsSeparator(preChar))
                        {
                            preChar = symbol;
                            if (word != string.Empty)
                            {
                                yield return word;
                                word = string.Empty;
                            }
                            continue;
                        }
                        else
                        {
                            preChar = symbol;
                            word += symbol;
                            continue;
                        }
                }

                if (preChar == '\'')
                {
                    if (char.IsPunctuation(symbol) || char.IsSeparator(symbol))
                    {
                        if (word != string.Empty && word.Last() == '\'')
                        {
                            word = word.Substring(0, word.Length - 1);
                        }

                        preChar = symbol;
                        if (word != string.Empty)
                        {
                            yield return word;
                            word = string.Empty;
                        }
                        continue;
                    }
                    else
                    {
                        preChar = symbol;
                        word += symbol;
                        continue;
                    }
                }

                if (separators.Contains(symbol))
                {
                    preChar = symbol;

                    if (word != string.Empty)
                    {
                        yield return word;
                        word = string.Empty;
                    }
                    continue;
                }

                preChar = symbol;
                word += symbol;
            }

            if (word != string.Empty)
            {
                yield return word;
                word = string.Empty;
            }

            yield break;
        }

        private IEnumerable<char> GetChars(IInputData inputData, bool ignoreSpecSymbols = true)
        {
            AnalyzerFlags flags = AnalyzerFlags.None;
            string specSymbol = string.Empty;
            short maxSpecSymbolLength = 40;
            short counterSpecSymbolLength = 0;

            foreach (char symbol in inputData)
            {
                switch (symbol)
                {
                    case '>':
                        if (flags.HasFlag(AnalyzerFlags.WaitBeginText))
                        {
                            flags ^= AnalyzerFlags.WaitBeginText;
                        }
                        flags |= AnalyzerFlags.WaitEndText;
                        break;
                    case '<':
                        if (flags.HasFlag(AnalyzerFlags.WaitEndText))
                        {
                            flags ^= AnalyzerFlags.WaitEndText;
                        }
                        flags |= AnalyzerFlags.WaitBeginText;
                        break;
                    case '&':
                        flags |= AnalyzerFlags.WaitEndSpecSymbol;
                        specSymbol += symbol;
                        counterSpecSymbolLength++;
                        break;
                    case ';':
                        if (flags.HasFlag(AnalyzerFlags.WaitEndSpecSymbol))
                        {
                            specSymbol += symbol;
                            flags ^= AnalyzerFlags.WaitEndSpecSymbol;

                            if (!ignoreSpecSymbols)
                            {
                                foreach (var ch in specSymbol)
                                {
                                    yield return ch;
                                }
                            }
                            else
                            {
                                yield return SpecSymbolReplace(specSymbol);
                            }

                            specSymbol = string.Empty;
                            counterSpecSymbolLength = 0;
                            continue;
                        }
                        if (flags.HasFlag(AnalyzerFlags.WaitEndText))
                        {
                            yield return symbol;
                            continue;
                        }
                        break;
                    default:
                        {
                            if (flags.HasFlag(AnalyzerFlags.WaitEndSpecSymbol))
                            {
                                specSymbol += symbol;
                                counterSpecSymbolLength++;

                                if (counterSpecSymbolLength > maxSpecSymbolLength)
                                {
                                    foreach (var ch in specSymbol)
                                    {
                                        yield return ch;
                                    }

                                    flags ^= AnalyzerFlags.WaitEndSpecSymbol;
                                    specSymbol = string.Empty;
                                    counterSpecSymbolLength = 0;
                                }
                                continue;
                            }
                            if (flags.HasFlag(AnalyzerFlags.WaitEndText))
                            {
                                yield return symbol;
                                continue;
                            }
                            break;
                        }
                }
            }

            yield break;
        }

        private char SpecSymbolReplace(string specSymbol)
        {
            return specSymbol switch
            {
                "&rsquo;" => '\'',
                _ => ' '
            };
        }
    }
}
