using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StartKit
{


    public class FileAppender
    {
        private string _fileSuffix;
        private AppenderFreq _frequency;
        private string _folder;
        public const int RETRIES = 10;

        public FileAppender(string folder, string suffix,
                            AppenderFreq freq = AppenderFreq.DAILY)  {
            _fileSuffix = suffix;
            _frequency = freq;
            _folder = folder;
        }


        public void LogLine(string msg)
        {
            bool Done = false;
            for (int i = 0; i < RETRIES && !Done; i++) {
                try
                {
                    var fname = FileName;
                    File.AppendAllText(fname, msg);
                    Done = true;
                    break;
                }
                catch(IOException)
                {
                    Thread.Sleep(50);
                    continue;
                }
            }
        }

        public void Append(string msg)
        {
            LogLine(msg);
        }


        public string FileName
        {
            get
            {
                var time = DateTime.Now;
                if (_frequency == AppenderFreq.DAILY)
                {
                    return String.Format("{4}{0}{1}{2}_{3}", time.Year.ToString("D4"),
                                                             time.Month.ToString("D2"),
                                                             time.Day.ToString("D2"),
                                                             _fileSuffix, _folder);
                }
                else if (_frequency == AppenderFreq.HOURLY)
                    return String.Format("{5}{0}{1}{2}{3}_{4}", time.Year.ToString("D4"),
                                                      time.Month.ToString("D2"),
                                                      time.Day.ToString("D2"),
                                                      time.Hour.ToString("D2"),
                                                      _fileSuffix, _folder);
                else
                    return String.Format("{1}{0}", _fileSuffix, _folder);
            }
        }



    }
}
