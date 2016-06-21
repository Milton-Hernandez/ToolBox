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
using System.Diagnostics;
using System.Threading;
namespace Logging {
    public static class Timer     {
        #region private attributes
         private static Dictionary<int,Dictionary<string, double>> Map = 
                                 new Dictionary<int,Dictionary<string, double>>();
  
         private static Dictionary<int,Stopwatch> sw = 
                                 new Dictionary<int,Stopwatch>();
         
         private static Dictionary<int,string> current = 
                                 new Dictionary<int,string>();
         
         private static Stopwatch Global = new Stopwatch();
         private static string LINE = "________________________________________" +
                                      "_______________________________________\n";
        #endregion

        public static void Reset()  {
            Map = new Dictionary<int,Dictionary<string, double>>();
            sw = new Dictionary<int, Stopwatch>();
            current = new Dictionary<int, string>();
            Global.Reset();
            Global.Start();
        }

        public static void CheckPoint(string arg)  {
            lock (Map) {
                var x = Thread.CurrentThread.ManagedThreadId;
                if (!sw.ContainsKey(x)) {
                    sw.Add(x, new Stopwatch());
                    current[x] = "Init";
                    sw[x].Start();
                }
                double time = sw[x].ElapsedMilliseconds;
                if (!Map.ContainsKey(x))
                    Map[x] = new Dictionary<string, double>();
                if (!Map[x].ContainsKey(current[x]))
                    Map[x][current[x]] = 0.0;
                Map[x][current[x]] = Map[x][current[x]] + time;
                current[x] = arg;
                sw[x].Restart();
            }
        }

        public static string BarOfSize(double size) {
            int s = (int) Math.Floor(size);
            string ret = String.Format(": {0,2}% :|", s);
            for (int i = 0; i < s; i++)
                ret += "|";
            return ret;
        }

        public static string Pad(string arg, int size) {
            string ret = arg;
            int diff = size - arg.Length;
            for (int i = 0; i < diff; i++)
                ret += " ";
            return ret;
        }

        public static void Write(ConsoleColor color, string str) {
           Console.ForegroundColor = color;
           Console.Out.Write(str);
        }

        public static void WriteLine(ConsoleColor color, string str) {
            Write(color, str + '\n');
        }

        public static void ToConsole(String title, List<String> stats) {
            lock (LINE)
            {
                Console.Out.WriteLine("");
                Console.Out.WriteLine("Performance Stats: " + title);
                WriteLine(ConsoleColor.DarkGreen, LINE);
                for (int i = 0; i < 3; i++)
                {
                    String[] Token = stats[i].Split(':');
                    Write(ConsoleColor.DarkGreen, Token[0]);
                    WriteLine(ConsoleColor.Red, Token[1]);
                }
                WriteLine(ConsoleColor.DarkGreen, LINE);

                for (int i = 3; i < stats.Count; i++)
                {
                    String[] Token = stats[i].Split(':');
                    Write(ConsoleColor.DarkGreen, Token[0]);
                    Write(ConsoleColor.Red, Token[1]);
                    WriteLine(ConsoleColor.Blue, Token[2]);
                }
                WriteLine(ConsoleColor.DarkGreen, LINE);
                Console.ResetColor();
                Console.Out.WriteLine("");
            }
        }

        public static List<string> Stats() {
            #region A. Setup variables...
                var x = current.Keys.ToList<int>();
                for (int i = 0; i < x.Count; i++ )
                    CheckPoint(current[x[i]]);
                List<string> ret = new List<string>();
                double total = 0.0;
                int MaxSize = 10;
                double ellapsed = Global.ElapsedMilliseconds;
                Dictionary<int, double> totals = new Dictionary<int, double>();
                double tot = 0.0;
            #endregion
            #region B. Calc. Total duration... 
              foreach (var th in Map.Keys) {
                foreach (var key in Map[th].Keys) {
                    tot += Map[th][key];
                    if (key.Length > MaxSize)
                        MaxSize = key.Length;
                }
              }
            #endregion
            #region C. Write Header...
              ret.Add("  Real time:\t" + String.Format("{0:000.0} Ms", ellapsed));
              ret.Add("  Thread Count:\t" + String.Format("{0}", Map.Count));
              ret.Add("  Logical time:\t" + String.Format("{0:000.0} Ms", tot));
              var Flat = new Dictionary<string, double>();
            #endregion
            #region D. Write Bar Chart...
              foreach (var th in Map.Keys) {
                foreach (var key in Map[th].Keys)   {
                    var slice = (Map[th][key] / tot) * 100.0;
                    if (slice < 1)
                        continue;
                    if (!Flat.ContainsKey(key))
                        Flat[key] = 0.0;
                    Flat[key] = Flat[key] + slice;
                }
              }
             foreach (var key in Flat.Keys)  {
                double slice = Flat[key];
                ret.Add("  " + Pad(key,MaxSize+3) +  BarOfSize(slice) );
             }
            #endregion          
            Reset(); 
            return ret;
        }
    }
}
