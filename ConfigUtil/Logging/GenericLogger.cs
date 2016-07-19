using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public sealed class GenericLogger
    {
        public LevelType LogLevel { get; set; }
        public int Instance { get { return Runtime.ThisInstance + 1; } }
        public ILogger InLogger { get; set; }

        public void Log(object arg)
        {
            if (LogLevel == LevelType.DEBUG)
                InLogger.Debug(arg);
            else if (LogLevel == LevelType.INFO)
                InLogger.Info(arg);
            else if (LogLevel == LevelType.WARNING)
                InLogger.Warn(arg);
            else 
                InLogger.Error(arg);
        }

        public void LogAll(IEnumerable args)
        {
            foreach (var item in args)
                Log(item);
        }

        public void LogAll(string format, IEnumerable args)
        {
            foreach (var item in args)
                Log(format,item);
        }

        public void Log(string format, params object[] varargs) {
            if (LogLevel == LevelType.DEBUG)
                InLogger.Debug(format, varargs);
            else if (LogLevel == LevelType.INFO)
                InLogger.Info(format, varargs);
            else if (LogLevel == LevelType.WARNING)
                InLogger.Warn(format, varargs);
            else 
                InLogger.Error(format, varargs);
        }

       public  void Log(string arg, Exception ex)
        {
            if (LogLevel == LevelType.DEBUG)
                InLogger.Debug(arg,ex);
            else if (LogLevel == LevelType.INFO)
                InLogger.Info(arg,ex);
            else if (LogLevel == LevelType.WARNING)
                InLogger.Warn(arg,ex);
            else
                 InLogger.Error(arg,ex);
        }
    }
}
