using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WordAnalyzer.Analyzer.InputData;
using WordAnalyzer.Common;
using WordAnalyzer.Control;
using WordAnalyzer.Presentators.Console;

namespace WordAnalyzer.IO
{
    public class ConsoleIO : IConsoleIO
    {
        private static readonly string _dataPath = @"Data\";
        private static readonly IEnumerable<string> _dataExtensions = new List<string> { ".html" };
        private readonly ILogger<ConsoleIO> _logger;
        private readonly IDataPresentator _dataPresentator;
        private readonly WordAnalyzerController _wordAnalyzerController;

        public ConsoleIO(
            ILogger<ConsoleIO> logger,
            IDataPresentator dataPresentator,
            WordAnalyzerController wordAnalyzerController)
        {
            _logger = logger;
            _dataPresentator = dataPresentator;
            _wordAnalyzerController = wordAnalyzerController;
        }

        public void ListenerRun()
        {
            Listen(Disclaimer, null);
            Listen(Menu, null);
        }

        private MenuResponse Disclaimer(Dictionary<string, object>? parameters)
        {
            ScreenShow("Disclaimer\n", null);
            ConsolePresentator.WriteLine(new ConsolePresentatorItem()
            {
                Text = "Если в загружаемых html документах присутствуют какие-либо синтаксичесие ошибки,\n" +
                       "то возможен некорректный анализ текста.\n" +
                       "Анализ может включать текст кнопок и других элементов.\n" +
                       "Будьте внимательны.\n",
                Type = TextType.Info
            });
            _ = GetInput<object>("Нажмите любую клавишу...", (result) => new InputValidationResponse
            {
                IsValid = true,
                TextError = default
            });
                
            return new MenuResponse();
        }

        private MenuResponse Menu(Dictionary<string, object>? parameters)
        {
            List<MenuItem> items = new()
            {
                new MenuItem
                {
                    Index = 1,
                    Title = "Загрузить данные"
                },
                new MenuItem
                {
                    Index = 2,
                    Title = "Анализ"
                },
                new MenuItem
                {
                    Index = 3,
                    Title = "Закрыть"
                }
            };

            ScreenShow("Меню\n", items);

            var command = GetInput<int?>("Введите ваш выбор: ", (result) =>
            {
                if (!result.HasValue)
                {
                    return new InputValidationResponse
                    {
                        IsValid = false,
                        TextError = "Вы ничего не ввели.\n"
                    };
                }
                else if (!items.Select(x => x.Index).Contains(result.Value))
                {
                    return new InputValidationResponse
                    {
                        IsValid = false,
                        TextError = "Такой комманды нет.\n"
                    };
                }

                return new InputValidationResponse
                {
                    IsValid = true,
                    TextError = default
                };
            })!.Value;

            Func<Dictionary<string, object>?, MenuResponse>? next = (Screen) command switch
            {
                Screen.SelectingInput => SelectingInput,
                Screen.Analyze => Analyze,
                Screen.Close => Close,
                _ => default
            };

            return new MenuResponse
            {
                Next = next,
                Parameters = parameters
            };
        }

        private MenuResponse SelectingInput(Dictionary<string, object>? parameters)
        {
            List<MenuItem> items = new();
            Dictionary<MenuItem, FileInfo> itemFiles = new();
            var fileInfos = GetFiles(_dataPath, _dataExtensions).OrderBy(x => x.Extension).ThenBy(x => x.Name);
            var index = 1;
            foreach (var fileInfo in fileInfos)
            {
                MenuItem menuItem = new()
                {
                    Index = index++,
                    Title = fileInfo.Name
                };

                items.Add(menuItem);
                itemFiles.Add(menuItem, fileInfo);
            }
            items.Add(new MenuItem
            {
                Index = index++,
                Title = "Ввести свой путь до файла"
            });

            ScreenShow("Загрузка данных\n", items);

            var command = GetInput<int?>("Введите ваш выбор: ", (result) =>
            {
                if (!result.HasValue)
                {
                    return new InputValidationResponse
                    {
                        IsValid = false,
                        TextError = "Вы ничего не ввели.\n"
                    };
                }
                else if (!items.Select(x => x.Index).Contains(result.Value))
                {
                    return new InputValidationResponse
                    {
                        IsValid = false,
                        TextError = "Такой комманды нет.\n"
                    };
                }

                return new InputValidationResponse
                {
                    IsValid = true,
                    TextError = default
                };
            })!.Value;

            string path;
            if (items.Last().Index != command)
            {
                path = itemFiles[items.First(x => x.Index == command)].FullName;
            }
            else
            {
                path = GetInput<string>("Введите путь до файла: ", (result) =>
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        return new InputValidationResponse
                        {
                            IsValid = false,
                            TextError = "Некорректная команда.\n"
                        };
                    }
                    else if (!File.Exists(result))
                    {
                        return new InputValidationResponse
                        {
                            IsValid = false,
                            TextError = "Такого файла не существует.\n"
                        };
                    }

                    return new InputValidationResponse
                    {
                        IsValid = true,
                        TextError = default
                    };
                })!;
            }

            FileInputData fileInputData = new(path);
            if (parameters?.ContainsKey(ParameterConsts.INPUT_DATA) ?? false)
            {
                parameters[ParameterConsts.INPUT_DATA] = fileInputData;
            }
            else
            {
                parameters ??= new Dictionary<string, object>();

                parameters.Add(ParameterConsts.INPUT_DATA, fileInputData);
            }

            return new MenuResponse
            {
                Next = Menu,
                Parameters = parameters
            };
        }

        private MenuResponse Analyze(Dictionary<string, object>? parameters)
        {
            ScreenShow("Анализ\n", null);

            if (!parameters?.ContainsKey(ParameterConsts.INPUT_DATA) ?? true)
            {
                ConsolePresentator.WriteLine(new ConsolePresentatorItem
                {
                    Text = "Нет входных данных\n",
                    Type = TextType.Error
                });
                _ = GetInput<object>("Нажмите любую клавишу, чтобы вернуться в главное меню...", (result) => new InputValidationResponse
                {
                    IsValid = true,
                    TextError = default
                });

                return new MenuResponse
                {
                    Next = Menu,
                    Parameters = parameters
                };
            }

            var inputData = (IInputData)parameters![ParameterConsts.INPUT_DATA];
            var statistics = _wordAnalyzerController.Analyze(inputData);
            
            if (statistics != null)
            {
                _dataPresentator.Show(statistics);
            }
            else
            {
                ConsolePresentator.WriteLine(new ConsolePresentatorItem
                {
                    Text = "Возникла ошибка во время анализа документа.\n",
                    Type = TextType.Error
                });
            }

            _ = GetInput<object>("Нажмите любую клавишу, чтобы вернуться в главное меню...", (result) => new InputValidationResponse
            {
                IsValid = true,
                TextError = default
            });

            return new MenuResponse
            {
                Next = Menu,
                Parameters = parameters
            };
        }

        private MenuResponse Close(Dictionary<string, object>? parameters)
        {
            return new MenuResponse();
        }

        private T? GetInput<T>(string requestText, Func<T?, InputValidationResponse> validation)
        {
            try
            {
                T? result = default;
                var isCorrectInput = true;
                do
                {
                    isCorrectInput = true;
                    ConsolePresentator.WriteLine(new ConsolePresentatorItem
                    {
                        Text = requestText,
                        Type = TextType.Control
                    });
                    var command = ConsolePresentator.ReadLine(TextType.Control);
                    ConvertTypes.TryChangeType<T>(command, out result);
                    var validationResponse = validation.Invoke(result);
                    if (!string.IsNullOrEmpty(validationResponse.TextError))
                    {
                        ConsolePresentator.WriteLine(new ConsolePresentatorItem
                        {
                            Text = validationResponse.TextError,
                            Type = TextType.Error
                        });
                        isCorrectInput = false;
                    }
                } while (!isCorrectInput);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unknown error.");

                ConsolePresentator.WriteLine(new ConsolePresentatorItem
                {
                    Text = "Что-то пошло не так((\n",
                    Type = TextType.Error
                });
                Console.ReadKey();

                return default;
            }
        }

        private void ScreenShow(string title, IEnumerable<MenuItem>? items)
        {
            Console.Clear();
            ConsolePresentator.WriteLine(new ConsolePresentatorItem
            {
                Text = title,
                Type = TextType.UI
            });

            if (items == null || !items.Any())
            {
                return;
            }

            string menuItems = string.Join('\n', items.Select(x => $"{x.Index}. {x.Title}")) + "\n";
            ConsolePresentator.WriteLine(new ConsolePresentatorItem
            {
                Text = menuItems,
                Type = TextType.Info
            });
        }

        private void Listen(Func<Dictionary<string, object>?, MenuResponse>? func, Dictionary<string, object>? parameters)
        {
            var isRun = true;

            while (isRun)
            {
                var menuResponse = func?.Invoke(parameters);
                if (menuResponse?.Next == default)
                {
                    isRun = false;
                }

                func = menuResponse?.Next;
                parameters = menuResponse?.Parameters;
            }
        }

        private IEnumerable<FileInfo> GetFiles(string folder, IEnumerable<string> extensions)
        {
            List<FileInfo> result = new();
            foreach (var file in Directory.EnumerateFiles(folder))
            {
                FileInfo fileInfo = new(file);
                if (extensions.Contains(fileInfo.Extension))
                {
                    result.Add(fileInfo);
                }
            }

            return result;
        }
    }
}