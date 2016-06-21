using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging {
    public static class Log     {
     #region Implementation
        private static Logger _logger;

        private static  NLog.LogLevel GetLevel(LevelType arg)  {
            if (arg == LevelType.DEBUG)
                return LogLevel.Debug;
            else if (arg == LevelType.ERROR)
                return LogLevel.Error;
            else if (arg == LevelType.INFO)
                return LogLevel.Info;
            return LogLevel.Warn;
        }

        private static FileTarget GetFileTarget() {
          return new FileTarget
                     {
                         FileName = Config.Global.LogFolder + Path.DirectorySeparatorChar + Config.Global.LogName,
                         EnableFileDelete = true,
                         ArchiveAboveSize = 1024 * 1024 * 100,
                         CreateDirs = true,
                         ArchiveNumbering = ArchiveNumberingMode.Rolling
                     };
         }

        private static void PrintException(string arg, Exception ex, Action<string> log)   {
            log("--------------------------------Exception------------------------------");
            log(arg);
            log("-----------------------------------------------------------------------");
            log(ex.Message);
            log("=======================================================================");
            string[] lines = ex.StackTrace.Split('\n');
            foreach (var line in lines)
                log(line.Trim());
            log("-----------------------------------------------------------------------");
            if (ex.InnerException != null)
                PrintException("Inner Exeption: ", ex.InnerException, log);
        }
     #endregion

     #region contract        
        public static void Debug(object arg)  {
         if(Config.Global.IsDebug)
            _logger.Debug(arg);
        }

        public static void Info(object arg)  {
            _logger.Info(arg);
        }

        public static void Warn(object arg)  {
            _logger.Warn(arg);
        }

        public static void Error(object arg) {
            _logger.Error(arg);
        }

        public static void Fatal(object arg) {
            _logger.Fatal(arg);
        }

        public static void Debug(string arg, Exception ex)  {
            if (Config.Global.IsDebug) 
                PrintException(arg, ex, _logger.Debug);
        }

        public static void Info(string arg, Exception ex) {
            PrintException(arg, ex, _logger.Info); 
        }

        public static void Warn(string arg, Exception ex) {
            PrintException(arg, ex, _logger.Warn);
        }

        public static void Error(string arg, Exception ex) {
            PrintException(arg, ex, _logger.Error);
        }

        public static void Fatal(string arg, Exception ex) {
            PrintException(arg, ex, _logger.Fatal);
        }
     #endregion

     /// <summary>
        /// Static Constructor:  This is invoked when the class is invoked for the first time,
        /// before everything else.
        /// </summary>
     static  Log() {
            Reset();
        }

     public static void Reset() {
            var config = new LoggingConfiguration();
            var fileTarget = GetFileTarget();
            var consoleTarget = new ColoredConsoleTarget();

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                ForegroundColor = ConsoleOutputColor.DarkGreen,
                BackgroundColor = ConsoleOutputColor.Black,
                Condition = "level == LogLevel.Info"
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                ForegroundColor = ConsoleOutputColor.Red,
                BackgroundColor = ConsoleOutputColor.Black,
                Condition = "level == LogLevel.Error"
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                ForegroundColor = ConsoleOutputColor.White,
                BackgroundColor = ConsoleOutputColor.DarkRed,
                Condition = "level == LogLevel.Fatal"
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule()
            {
                ForegroundColor = ConsoleOutputColor.Yellow,
                BackgroundColor = ConsoleOutputColor.Black,
                Condition = "level == LogLevel.Warn"
            });

            consoleTarget.Layout = Config.Global.ConsoleLayout;
            fileTarget.Layout = Config.Global.FileLayout;


            if (Config.Global.LogToFile)
                config.AddTarget("logfile", fileTarget);

            if (Config.Global.LogToConsole)
            {
                config.AddTarget("console", consoleTarget);
                var Rule1 = new LoggingRule("*", GetLevel(Config.Global.Level), consoleTarget);
                config.LoggingRules.Add(Rule1);
            }

            if (Config.Global.LogToFile)
                config.LoggingRules.Add(new LoggingRule("*", GetLevel(Config.Global.Level), fileTarget));
            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }
    }
}
