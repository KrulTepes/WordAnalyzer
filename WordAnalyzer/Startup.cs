using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WordAnalyzer.Analyzer;
using WordAnalyzer.Analyzer.Parsers;
using WordAnalyzer.Control;
using WordAnalyzer.DataAccess;
using WordAnalyzer.IO;
using WordAnalyzer.Presentators.Console;
using WordAnalyzer.Repository;
using WordAnalyzer.Views.Console;

namespace WordAnalyzer
{
    internal class Startup
    {
        private static NLog.ILogger? _logger;
        private static IServiceProvider? _serviceProvider;
        private static void Main(string[] args)
        {
            Console.WriteLine("Start");

            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug("Start");

            // загружать данные с диска пакетами сохраняя в буфер

            // вынести анализ в отдельный процесс с получением данных о прогрессе

            // используя AngleSharp, переписать его парсер использования его посимвольной обработки не загружая файл целиком.
            // Удаляя закончившиеся ноды и всю ненужную информацию.
            // Доставая plaintext моментально его валидируя и внося в статистику, либо пробрасывать итератор.


            // мейби локализацию на советский ;)

            // потестить на всякие необычные ошибки по типу файл был файла нет чё делать

            /*на финал
             усложнение выборки. 
                Будет анализироваться не весь текст, а отдельные его части, например только div, ссылки и тп
                Возможны исключения. То есть проализировать весь текст, кроме отдельных его блоков.
             */
            
            
            ExceptionMiddleware(Configure);
            ExceptionMiddleware(Run);
            ExceptionMiddleware(Stop);
            _logger.Debug("End");
        }

        private static void Run()
        {
            if (_serviceProvider == null)
            {
                throw new NullReferenceException("ServiceProvider is null.");
            }
            
            var consoleIO = _serviceProvider.GetRequiredService<IConsoleIO>();

            consoleIO.ListenerRun();
        }

        private static void Stop()
        {
            Task.WaitAll();
        }

        private static void Configure()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
                .AddJsonFile("wordAnalyzerSettings.json", optional: true, reloadOnChange: true)
                .Build();

            var serviceCollection = new ServiceCollection()
               .AddLogging(loggingBuilder =>
               {
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(config);
               });

            serviceCollection.AddSingleton<IDapperSettings>(x => new DapperSettings(
                config.GetValue<string>($"{nameof(DapperSettings)}:{nameof(DapperSettings.ConnectionString)}"),
                config.GetValue<Provider>($"{nameof(DapperSettings)}:{nameof(DapperSettings.Provider)}")
            ));
            serviceCollection.AddScoped(typeof(IDapperContext), typeof(DapperContext));
            serviceCollection.AddTransient(typeof(IStatisticsRepository), typeof(StatisticsRepository));
            serviceCollection.AddTransient<IDataConsoleView>(x => new FrequencyOccurrenceView(TextType.Info, OrderBy.DESC));
            serviceCollection.AddTransient(typeof(IDataPresentator), typeof(ConsolePresentator));
            serviceCollection.AddTransient(typeof(IDataAnalyzerFactory), typeof(HtmlAngelSharpDataAnalyzerFactory));
            serviceCollection.AddSingleton<WordAnalyzerController>();
            serviceCollection.AddSingleton(typeof(IConsoleIO), typeof(ConsoleIO));

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ExceptionMiddleware(Action func)
        {
            try
            {
                func.Invoke();
            }
            catch (Exception e)
            {
                _logger?.Error(e);
            }
        }
    }
}