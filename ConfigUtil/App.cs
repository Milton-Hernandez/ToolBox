using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StartKit;
using StartKit.Configuration;
using System.IO;

namespace StartKit
{
    public enum LevelType
    {
        ERROR,WARNING,INFO,DEBUG
    }

    public static class App
    {
        private static LevelType _level   = Runtime.Arguments.ContainsKey("LL") ?
                                            (LevelType) Enum.Parse(typeof(LevelType),Runtime.Arguments["LL"] ) :  LevelType.DEBUG;
        private static string _logFolder  = Runtime.Arguments.ContainsKey("LD") ? Runtime.Arguments["LD"] : Path.Combine(Runtime.OutputDir, "logs");
        private static string _logName    =   Runtime.Arguments.ContainsKey("LN") ? Runtime.Arguments["LN"] : Runtime.Name + ".log";
        private static bool _logToConsole = Runtime.Arguments.ContainsKey("LC") ? bool.Parse(Runtime.Arguments["LC"]) : true;
        private static bool _logToFile    = Runtime.Arguments.ContainsKey("LF") ? bool.Parse(Runtime.Arguments["LF"]) : true;
        private static string _consoleLayout = @"[${time}]: ${message}";
        private static string _fileLayout = @"[${longdate}] | ${pad:padding=5:inner=${level:uppercase=true}} | ${message}";



        [ShortcutAttribute("LL")] public static LevelType Level { get { return _level; } private set { _level = value; } }
        public static bool IsDebug {
            get { return _level == LevelType.DEBUG; }
        }
        [ShortcutAttribute("LD")] public static string LogFolder { get { return _logFolder; } private set { _logFolder = value; } }
        [ShortcutAttribute("LN")] public static string LogName { get { return _logName; } private set { _logName = value; } }
        [ShortcutAttribute("LC")] public static bool LogToConsole { get { return _logToConsole; } private set { _logToConsole = value; } }
        [ShortcutAttribute("LF")] public static bool LogToFile { get { return _logToFile; } private set { _logToFile = value; } }

        public static string ConsoleLayout { get { return _consoleLayout; } private set { _consoleLayout = value; } }
        public static string FileLayout { get { return _fileLayout; } private set { _fileLayout = value; } }
        public static string AlertEmailAddress { get; private set; }
        public static string InfoEmailAddress { get; private set; }
        public static string SenderEmailAddress { get; private set; }
        public static string SMTPHost { get; private set; }

        static App()
        {
            Settings.Load();
        }

        private static ILogger _log;

        public static ILogger Log
        {
            get
            {
                if (_log == null)
                    _log = new NLogLogger();
                return _log;
            }
        }

    }
}
