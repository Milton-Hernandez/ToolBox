using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StartKit;
using StartKit.Configuration;
using System.IO;
using StartKit.Serialization;

namespace StartKit
{
    public enum LevelType
    {
        ERROR,WARNING,INFO,DEBUG
    }

    public enum AppenderFreq
    {
        HOURLY,
        DAILY,
        OFF
    }

    public static class App
    {
        internal static bool WatchProperties = false;

        private static LevelType _level   = Runtime.Arguments.ContainsKey("LL") ?
                                            (LevelType) Enum.Parse(typeof(LevelType),Runtime.Arguments["LL"] ) :  LevelType.DEBUG;
        private static string _logFolder  = Runtime.Arguments.ContainsKey("LD") ? Runtime.Arguments["LD"] : Path.Combine(Runtime.OutputDir, "logs");
        private static string _logName    =   Runtime.Arguments.ContainsKey("LN") ? Runtime.Arguments["LN"] : Runtime.Name + ".log";
        private static bool _logToConsole = Runtime.Arguments.ContainsKey("LC") ? bool.Parse(Runtime.Arguments["LC"]) : true;
        private static bool _logToFile    = Runtime.Arguments.ContainsKey("LF") ? bool.Parse(Runtime.Arguments["LF"]) : false;
        private static string _consoleLayout = @"[${time}]: ${message}";
        private static string _fileLayout = @"[${longdate}] | ${pad:padding=5:inner=${level:uppercase=true}} | ${message}";
        private static AppenderFreq _freq = Runtime.Arguments.ContainsKey("LFreq") ?
                                           (AppenderFreq)Enum.Parse(typeof(AppenderFreq), Runtime.Arguments["LFreq"]) :
                                           AppenderFreq.DAILY;
        private static Profiler _timer;
        public static readonly dynamic Cat = CategoryFinder.Cat;

        [ShortcutAttribute("LL")]
        public static LevelType Level {
            get { return _level; } 
            set {
                _level = value;
               ResetLog();
            }
        }

        [ShortcutAttribute("LFreq")]
        public static AppenderFreq Fequency
        {
            get { return _freq; }
            set
            {
                _freq = value;
                ResetLog();
            }
        }


        public static Profiler Timer
        {
            get
            {
                return _timer;
            }
        }

        public static bool IsDebug {
            get { return _level == LevelType.DEBUG; }
        }

        [ShortcutAttribute("LD")]
        public static string LogFolder
        {
             get {
                  return _logFolder;
            }
             set { 
                  _logFolder = value;
                ResetLog();
            }
        }

        [ShortcutAttribute("TM")]
        public static bool Profiling
        {
            get
            {
                return (_timer == null);
            }
            set
            {
                if (value)
                    _timer = new Profiler();
            }
        }


        [ShortcutAttribute("LN")]
        public static string LogName {
            get { return _logName; }
            set {
                _logName = value;
                ResetLog();
            }
        }

        [ShortcutAttribute("LC")]
        public static bool LogToConsole {
            get {
                return _logToConsole; }
            set {
                   _logToConsole = value;
                ResetLog();
            }
        }

        [ShortcutAttribute("LF")]
        public static bool LogToFile
        {
            get { return _logToFile; }
            set {
                _logToFile = value;
                ResetLog();
            }
        }

        public static string ConsoleLayout { get { return _consoleLayout; } private set { _consoleLayout = value; } }
        public static string FileLayout { get { return _fileLayout; } private set { _fileLayout = value; } }
        public static string AlertEmailAddress { get; private set; }
        public static string InfoEmailAddress { get; private set; }
        public static string SenderEmailAddress { get; private set; }
        public static string SMTPHost { get; private set; }

        static App()
        { 
            Settings.Load();
            SetLog(_level);
        }

        public static GenericLogger Debug { get; private set; }
        public static GenericLogger Info { get; private set; }
        public static GenericLogger Warning { get; private set; }
        public static GenericLogger Error { get; private set; }

        public static void ResetLog()
        {
            if (WatchProperties)
            {
                UnderLogger = null;
                SetLog(_level);
            }
        }

        public static void PrintInfo()
        {
            App.Debug?.Log("------------------------------------------------");
            App.Debug?.Log("Runtime Name:\t" + Runtime.Name);
            App.Debug?.Log("Config Folder:\t" + Runtime.ConfigDir);
            App.Debug?.Log("Log Folder:\t" + App.LogFolder);
            App.Debug?.Log("Log Name:\t" + App.LogName);
            App.Debug?.Log("------------------------------------------------");
            foreach (var s in AssemblyReflector.AssemblyDictionary.Keys)
                App.Debug?.Log(AssemblyReflector.AssemblyDictionary[s].Label);
            App.Debug?.Log("------------------------------------------------");
        }

        public static void DumpConfig(Type t)
        {
            dynamic d = DynamicHelper.ToDynamic(t);
            string j = DynSerializer.ToJason(d);
            string fname = Runtime.ConfigDir + t.FullName + ".cfg";
            File.WriteAllText(fname, j);
            App.Info?.Log("Config Written to: " + fname);
        }

        public static void DumpConfig(Object obj)
        {
            dynamic d = DynamicHelper.ToDynamic(obj);
            string j = DynSerializer.ToJason(d);
            Type t = obj.GetType();
            string fname = Runtime.ConfigDir + t.FullName + ".cfg";
            File.WriteAllText(fname, j);
            App.Info?.Log("Config Written to: " + fname);
        }

        private static  ILogger UnderLogger { get;  set;  }


        private static void SetLog(LevelType curLevel)
        {
            if(UnderLogger == null)
                UnderLogger = new BasicLogger();
            if (curLevel == LevelType.DEBUG)
                Debug = new GenericLogger() { InLogger = UnderLogger, LogLevel = LevelType.DEBUG };
            if (curLevel >= LevelType.INFO)
                Info = new GenericLogger() { InLogger = UnderLogger, LogLevel = LevelType.INFO };
            if (curLevel >= LevelType.WARNING)
                Warning = new GenericLogger() { InLogger = UnderLogger, LogLevel = LevelType.WARNING};
            Error = new GenericLogger() { InLogger = UnderLogger, LogLevel = LevelType.ERROR };
        }

    }
}
