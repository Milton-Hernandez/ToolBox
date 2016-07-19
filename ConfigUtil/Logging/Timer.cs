using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
namespace ToolBox {
    public class Profiler     {

        public static dynamic Point { get { return App.Cat.TimePoint; }  }
        #region private attributes
         private Dictionary<int,Dictionary<Discrete, double>> Map =  new Dictionary<int,Dictionary<Discrete, double>>();
         private Dictionary<int,Stopwatch> sw =  new Dictionary<int,Stopwatch>();         
         private Dictionary<int,Discrete> current =  new Dictionary<int,Discrete>();         
         private Stopwatch Global = new Stopwatch();
         private string LINE = "_____________________________\n";

        #endregion

        public void Reset()  {
                Map = new Dictionary<int, Dictionary<Discrete, double>>();
                sw = new Dictionary<int, Stopwatch>();
                current = new Dictionary<int, Discrete>();
                Global.Reset();
                Global.Start();
        }

        public void CheckPoint(Discrete arg)  {

                lock (Map)
                {
                    var x = Thread.CurrentThread.ManagedThreadId;
                    if (!sw.ContainsKey(x))
                    {
                        sw.Add(x, new Stopwatch());
                        current[x] = Profiler.Point.Init;
                        sw[x].Start();
                    }
                    double time = sw[x].ElapsedMilliseconds;
                    if (!Map.ContainsKey(x))
                        Map[x] = new Dictionary<Discrete, double>();
                    if (!Map[x].ContainsKey(current[x]))
                        Map[x][current[x]] = 0.0;
                    Map[x][current[x]] = Map[x][current[x]] + time;
                    current[x] = arg;
                    sw[x].Restart();
                }

        }

        public string BarOfSize(double size) {
            int s = (int) Math.Floor(size);
            string ret = String.Format(": {0,2}% :|", s);
            for (int i = 0; i < s; i++)
                ret += "|";
            return ret;
        }

        public string Pad(string arg, int size) {
            string ret = arg;
            int diff = size - arg.Length;
            for (int i = 0; i < diff; i++)
                ret += " ";
            return ret;
        }

        public void Write(ConsoleColor color, string str) {
           Console.ForegroundColor = color;
           Console.Error.Write(str);
        }

        public void WriteLine(ConsoleColor color, string str) {
            Write(color, str + '\n');
        }

        public  void ToConsole(bool clear = false) {
            if (clear)
                Console.Clear();
            var stats = Stats();
            lock (LINE)
                {
                    ConsoleColor[] Colors = { ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.DarkYellow };

                    Console.Error.WriteLine("");
                    WriteLine(ConsoleColor.DarkGreen, LINE);
                    for (int i = 0; i < 3; i++)
                    {
                        String[] Token = stats[i].Split(':');
                        Write(ConsoleColor.DarkGreen, Token[0]);
                        WriteLine(ConsoleColor.Gray, Token[1]);
                    }
                    WriteLine(ConsoleColor.DarkGreen, LINE);

                    for (int i = 3; i < stats.Count; i++)
                    {
                        int p = (i - 3) % Colors.Length;
                        String[] Token = stats[i].Split(':');
                        Write(ConsoleColor.Gray, Token[0]);
                        Write(ConsoleColor.DarkGreen, Token[1]);
                        WriteLine(Colors[p], Token[2]);
                    }
                    WriteLine(ConsoleColor.DarkGreen, LINE);
                    Console.ResetColor();
                    Console.Error.WriteLine("");
                }
        }

        public List<string> Stats() {

                #region A. Setup variables...
                var x = current.Keys.ToList<int>();
                for (int i = 0; i < x.Count; i++)
                    CheckPoint(current[x[i]]);
                List<string> ret = new List<string>();
                int MaxSize = 10;
                double ellapsed = Global.ElapsedMilliseconds;
                Dictionary<int, double> totals = new Dictionary<int, double>();
                double tot = 0.0;
                #endregion
                #region B. Calc. Total duration... 
                foreach (var th in Map.Keys)
                {
                    foreach (var key in Map[th].Keys)
                    {
                        tot += Map[th][key];
                        if (key.ToString().Length > MaxSize)
                            MaxSize = key.ToString().Length;
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
                foreach (var th in Map.Keys)
                {
                    foreach (var key in Map[th].Keys)
                    {
                        var slice = (Map[th][key] / tot) * 100.0;
                        if (slice < 1)
                            continue;
                        if (!Flat.ContainsKey(key.ToString()))
                            Flat[key.ToString()] = 0.0;
                        Flat[key.ToString()] = Flat[key.ToString()] + slice;
                    }
                }
                foreach (var key in Flat.Keys)
                {
                    double slice = Flat[key];
                    ret.Add("  " + Pad(key, MaxSize + 3) + BarOfSize(slice));
                }
                #endregion
                Reset();
                return ret;
        }
    }
}
