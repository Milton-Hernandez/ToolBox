using System;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;
using System.Runtime.CompilerServices;

namespace StartKit
{
    public sealed class NLogLogger: ILogger
    {
        private static Logger _logger;
        private readonly bool _isDebug = App.IsDebug;
        


        private NLog.LogLevel GetLevel(LevelType arg)
        {
            if (arg == LevelType.DEBUG)
                return LogLevel.Debug;
            else if (arg == LevelType.ERROR)
                return LogLevel.Error;
            else if (arg == LevelType.INFO)
                return LogLevel.Info;
            return LogLevel.Warn;
        }
        private FileTarget GetFileTarget()
        {
            return new FileTarget
            {
                FileName = App.LogFolder + Path.DirectorySeparatorChar + App.LogName,
                EnableFileDelete = true,
                ArchiveAboveSize = 1024 * 1024 * 100,
                CreateDirs = true,
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };
        }
        private static void PrintException(string arg, Exception ex, Action<string> log)
        {
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

        public LevelType Level { get; set; }

        public NLogLogger()
        {
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

            consoleTarget.Layout = App.ConsoleLayout;
            consoleTarget.ErrorStream = true;
            fileTarget.Layout = App.FileLayout;


            if (App.LogToFile)
                config.AddTarget("logfile", fileTarget);

            if (App.LogToConsole)
            {
                config.AddTarget("console", consoleTarget);
                var Rule1 = new LoggingRule("*", GetLevel(App.Level), consoleTarget);
                config.LoggingRules.Add(Rule1);
            }

            if (App.LogToFile)
                config.LoggingRules.Add(new LoggingRule("*", GetLevel(App.Level), fileTarget));
            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }


        #region contract    


        public object Clone() { return this; }

        public  int Instance { get { return Runtime.ThisInstance + 1; } }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Debug(object arg)
        {
            Task.Run(() =>
            {
                if (_isDebug)
                {
                    if (Runtime.MaxInstances > 0)
                        _logger.Debug(String.Format("[INST-{0}] | {1}", Instance, arg));
                    else
                        _logger.Debug(arg);
                }
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Info(object arg)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                    _logger.Info(String.Format("[INST-{0}] | {1}", Instance, arg));
                else
                    _logger.Info(arg);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Warn(object arg)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                _logger.Warn(String.Format("[INST-{0}] | {1}", Instance, arg));
            else
                _logger.Warn(arg);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Error(object arg)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                _logger.Error(String.Format("[INST-{0}] | {1}", Instance, arg));
            else
                _logger.Error(arg);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fatal(object arg)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                _logger.Fatal(String.Format("[INST-{0}] | {1}", Instance, arg));
            else
                _logger.Fatal(arg);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Debug(string format, params object[] varargs)
        {
            Task.Run(() =>
            {
            if (_isDebug)
            {
                if (Runtime.MaxInstances > 0)
                    _logger.Debug("[INST-{0}] | {1}", Instance, String.Format(format, varargs));
                else
                    _logger.Debug(format, varargs);

             }
           });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Info(string format, params object[] varargs)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                _logger.Info("[INST-{0}] | {1}", Instance, String.Format(format, varargs));
            else
                _logger.Info(format, varargs);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Warn(string format, params object[] varargs)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                _logger.Warn("[INST-{0}] | {1}", Instance, String.Format(format, varargs));
            else
                _logger.Warn(format, varargs);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Error(string format, params object[] varargs)
        {
            Task.Run(() =>
            {
             if (Runtime.MaxInstances > 0)
                _logger.Error("[INST-{0}] | {1}", Instance, String.Format(format, varargs));
             else
                _logger.Error(format, varargs);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fatal(string format, params object[] varargs)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                    _logger.Fatal("[INST-{0}] | {1}", Instance, String.Format(format, varargs));
                else
                    _logger.Fatal(format, varargs);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Debug(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                if (_isDebug)
                PrintException(arg, ex, _logger.Debug);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Info(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _logger.Info);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Warn(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _logger.Warn);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Error(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _logger.Error);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fatal(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _logger.Fatal);
            });
        }
        #endregion
    }
}
