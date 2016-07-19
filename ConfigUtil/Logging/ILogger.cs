using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public interface ILogger
    {
        int Instance { get; }
        void Debug(object arg);
        void Info(object arg);
        void Warn(object arg);
        void Error(object arg);
        void Fatal(object arg);

        void Debug(string format, params object[] varargs);
        void Info(string format, params object[] varargs);
        void Warn(string format, params object[] varargs);
        void Error(string format, params object[] varargs);
        void Fatal(string format, params object[] varargs);

        void Debug(string arg, Exception ex);
        void Info(string arg, Exception ex);
        void Warn(string arg, Exception ex);
        void Error(string arg, Exception ex);
        void Fatal(string arg, Exception ex);
    }
}
