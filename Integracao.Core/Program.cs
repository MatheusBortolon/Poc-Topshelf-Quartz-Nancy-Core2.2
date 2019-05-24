using NLog;
using NLog.Config;
using NLog.Targets;
using Topshelf;
using Topshelf.Nancy;

namespace Integracao.Core
{
    class Program
    {
        private static Logger _log;

        static void Main(string[] args)
        {
            InitializeLogging();

            _log = LogManager.GetLogger("main");
            _log.Info("Logging initialized");

            var host = SchedulerService.GetHost();
            _log.Info("Host created");

            host.Run();
        }

        private static void InitializeLogging()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget()
            {
                Layout = @"${date:format=HH\\:MM\\:ss} [${logger}]: ${message}",
            };
            config.AddTarget("console", consoleTarget);

            /*
            config.AddTarget("file", new FileTarget()
            {
                FileName = "${basedir}/file.txt",
                Layout = "${message}"
            });
            
            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);
            */

            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            LogManager.Configuration = config;
        }

    }

}