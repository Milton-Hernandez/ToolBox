using System;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace StartKit
{
    public sealed class BasicLogger: ILogger
    {
        private readonly bool _isDebug = App.IsDebug;

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
        private ConsolePrinter _console;
        private FileAppender   _file;


        public BasicLogger()
        {
            _console = new ConsolePrinter(" | ");
            _file = new FileAppender(App.LogFolder, App.LogName, AppenderFreq.HOURLY," | ");
        }



        private static ConsoleColor[] DebugColors = { ConsoleColor.Gray, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green };
        private static ConsoleColor[] InfoColors = { ConsoleColor.Gray, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan };
        private static ConsoleColor[] WarnColors = { ConsoleColor.Gray, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow };
        private static ConsoleColor[] ErrorColors = { ConsoleColor.Gray, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red };
        #region contract    


        public object Clone() { return this; }

        public  int Instance { get { return Runtime.ThisInstance + 1; } }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _debug(object arg)
        {
            Task.Run(() =>
            {
                if (_isDebug)
                {
                    if (Runtime.MaxInstances > 0)
                        _print(DebugColors,DebugStr,Instance.ToString(), arg.ToString());
                    else
                        _print(DebugColors,DebugStr,"-", arg.ToString());
                }
            });
        }

        private void _print(ConsoleColor[] col, params string[] arg)
        {
            var t = DateTime.Now;
            if (App.LogToConsole)
                _console.Print(t, col, arg);
            if(App.LogToFile)
               _file.Print(t, col, arg);
        }

        private const string DebugStr = "DEBUG  ";
        private const string InfoStr  = "INFO   ";
        private const string WarnStr  = "WARNING";
        private const string ErrorStr = "ERROR  ";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _info(object arg)
        {
         //   Task.Run(() =>
         //   {
                if (Runtime.MaxInstances > 0)
                    _print(InfoColors, InfoStr, Instance.ToString(), arg.ToString());
                else
                    _print(InfoColors, InfoStr, "-", arg.ToString());
         //   });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _warn(object arg)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                    _print(WarnColors,  WarnStr, Instance.ToString(), arg.ToString());
                else
                    _print(WarnColors,  WarnStr, "-", arg.ToString());
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _error(object arg)
        {
            Task.Run(() =>
            {
                if (Runtime.MaxInstances > 0)
                    _print(ErrorColors,  ErrorStr, Instance.ToString(), arg.ToString());
                else
                    _print(ErrorColors,  ErrorStr, "-", arg.ToString());
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]



        public void Debug(string format, params object[] varargs)
        {
          if (varargs.Length == 0)
                _debug(format);
          else
            _debug(String.Format(format, varargs));           
        }

        public void Info(string format, params object[] varargs)
        {
            if (varargs.Length == 0)
                _info(format);
            else
               _info(String.Format(format, varargs));
        }

  
        public void Warn(string format, params object[] varargs)
        {
            if (varargs.Length == 0)
               _warn(format);
            else
                _warn(String.Format(format, varargs));
        }


        public void Error(string format, params object[] varargs)
        {
            if (varargs.Length == 0)
                _error(format);
            else
                _error(String.Format(format, varargs));
        }


        public void Fatal(string format, params object[] varargs)
        {
            if (varargs.Length == 0)
                _error(format);
            else
                _error(String.Format(format, varargs));
        }

        public void Debug(object arg)
        {
            if (_isDebug)
                _debug(arg.ToString());
        }

        public void Info(object arg)
        {
            _info(arg.ToString());
        }


        public void Warn(object arg)
        {
            _warn(arg.ToString());
        }


        public void Error(object arg)
        {
            _error(arg.ToString());
        }


        public void Fatal(object arg)
        {
            _error(arg.ToString());
        }


        public void Debug(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                if (_isDebug)
                  PrintException(arg, ex, _debug);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Info(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _info);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Warn(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _warn);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Error(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _error);
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fatal(string arg, Exception ex)
        {
            Task.Run(() =>
            {
                PrintException(arg, ex, _error);
            });
        }
        #endregion
    }
}
