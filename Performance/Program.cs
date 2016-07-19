using ToolBox;
using System;
using ToolBox.Configuration;
using TestNamespace;
using System.Threading;

namespace Performance
{
    public class Program
    {
        static void Main(string[] args)
        {
         //   ConfigFileCreator.Create();
            var t = new Profiler();
            Runtime.SetArgs(args);

            App.Profiling = true;
            App.LogToFile = true;
            App.Info.Log("Logging Information");
            App.PrintInfo();
            App.Warning?.Log("An Additional warning");


            App.Debug?.Log("SMPT Host: " + App.SMTPHost);
            App.Debug?.Log("This is my parameter: {0}", App.IsDebug);
            Runtime.MaxInstances = 3;
            App.Debug?.Log("SMPT Host: " + App.SMTPHost);
            App.Debug?.Log("This is my parameter: {0}", App.IsDebug);
            App.Error?.Log("This Instance: " + Runtime.ThisInstance);
            //new stuff
            App.DumpConfig(typeof(TestNamespace.Properties));

            Console.In.ReadLine();
            App.LogToFile = true;
            App.Error?.Log("Prop1: " + Properties.Prop1);
            App.Error?.Log("Prop2: " + Properties.Prop2);
            App.Error?.Log("Prop3: " + Properties.Prop3);

            App.Error.Log("Log Folder: " + App.LogFolder);

          /*  var Colors = new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Red };

            var f = new CompositePrinter(new FileAppender(@"c:\temp\", "file", AppenderFreq.HOURLY), new ConsolePrinter(" | "));


            for (int i = 0; i < 1000; i++)
                f.Print(Colors, DateTime.Now.ToString(), ": Message #:" + i, "Extra Info","More Extra Info","Even More Extra info");

 */
            Console.In.ReadLine();
            for (;;)
            {
                App.Timer?.Reset();
                t.CheckPoint(Profiler.Point.First);
                App.Info?.Log("ConfigDir: " + Runtime.ConfigDir);
                t.CheckPoint(Profiler.Point.Point1);
                App.Info?.Log("{0}", App.LogToFile);

                string r = "";
                Random rnd = new Random();
                int N = rnd.Next() % 5000;
                App.Timer?.CheckPoint(Profiler.Point.Point2);
                for (int i = 0; i < N; i++)
                    foreach (var s in Properties.PropArray)
                        r += s;
                App.Timer?.CheckPoint(Profiler.Point.Point3);
                App.LogName = "newlogfile.log";
                App.Timer?.CheckPoint(Profiler.Point.Point4);

                r = "";
                for (int i = 0; i < N; i++)
                    foreach (var s in Properties.PropArray)
                        r += s;
                App.Timer?.CheckPoint(Profiler.Point.Point5);
                string input = @"This is an example of when we should #FIRST# and #SECOND# and #C:\THIRD# for the good of all";
                var toks = ConfigLoader.AllMatches(input);
                App.Timer?.CheckPoint(Profiler.Point.Point6);
                r = "";
                for (int i = 0; i < N; i++)
                    foreach (var s in toks)
                        r += "Token is " + s;
                App.Timer?.CheckPoint(Profiler.Point.Point7);
                App.Timer?.ToConsole();
                App.Timer?.Reset();
                Thread.Sleep(1000);
            };
        }
    }
}
