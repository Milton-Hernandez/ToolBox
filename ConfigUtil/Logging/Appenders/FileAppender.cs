using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StartKit
{
    public class FileAppender:  TextPrinter
    {
        private string _fileSuffix;
        private AppenderFreq _frequency;
        private string _folder;
        public const int RETRIES = 10;
        private string _suffix = ".log";
        
        public FileAppender(string folder, string suffix,
                            AppenderFreq freq = AppenderFreq.DAILY, string sep = " "): base(sep)  {
            _fileSuffix = suffix;
            _frequency = freq;
            _folder = folder;
            if (Runtime.ThisInstance > 0)
                _suffix = "." + Runtime.ThisInstance + ".log";
        }


        public override void Write(ConsoleColor color, string str)
        {
            File.AppendAllText(FileName, str);
        }

        public override string GetTime(DateTime t)
        {
            return t.ToString("yyyy-MM-dd HH:mm:ss.ffff");
        }

        public string FileName
        {
            get
            {
                var time = DateTime.Now;
                if (_frequency == AppenderFreq.DAILY)
                {
                    return String.Format("{4}\\{0}{1}{2}_{3}{5}", time.Year.ToString("D4"),
                                                             time.Month.ToString("D2"),
                                                             time.Day.ToString("D2"),
                                                             _fileSuffix, _folder,_suffix);
                }
                else if (_frequency == AppenderFreq.HOURLY)
                    return  String.Format("{5}\\{0}{1}{2}{3}_{4}{6}", time.Year.ToString("D4"),
                                                      time.Month.ToString("D2"),
                                                      time.Day.ToString("D2"),
                                                      time.Hour.ToString("D2"),
                                                      _fileSuffix, _folder,_suffix);
                else
                    return String.Format("{1}\\{0}{2}", _fileSuffix, _folder,_suffix);
            }
        }



    }
}
