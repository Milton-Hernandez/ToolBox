using StartKit;
using System;
using StartKit.Configuration;
using TestNamespace;
using System.Threading;

namespace Performance
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConfigFileCreator.Create();
            var t = new Profiler();
            Runtime.SetArgs(args);                                   
            Runtime.ConfigDir = Environment.GetEnvironmentVariable("TMP") + "/" + ".skconf/";

            App.Info.Log("Logging Information");
            App.LogToConsole = true;
            App.Profiling = true;
            App.Info?.Log("Runtime Name: "  + Runtime.Name);
            App.Info?.Log("Config Dir: " + Runtime.ConfigDir);
            App.Info?.Log(App.LogFolder);
            App.Info?.Log(App.LogName);



            App.Debug?.Log("SMPT Host: " + App.SMTPHost);
            App.Debug?.Log("This is my parameter: {0}", App.IsDebug);
            Runtime.MaxInstances = 3;
            App.Debug?.Log("SMPT Host: " + App.SMTPHost);
            App.Debug?.Log("This is my parameter: {0}", App.IsDebug);
            App.Error?.Log("This Instance: " + Runtime.ThisInstance);

            App.LogToFile = true;
            App.Error?.Log("Prop1: " + Properties.Prop1);
            App.Error?.Log("Prop2: " + Properties.Prop2);
            App.Error?.Log("Prop3: " + Properties.Prop3);

            App.Error.Log("Log Folder: " + App.LogFolder);

            var f = new FileAppender(@"d:\temp\","file.log",AppenderFreq.HOURLY);
            App.Info.Log(f.FileName);

            for (int i = 0; i < 1000; i++)
                f.Append(DateTime.Now.ToString() + ": Message #:" + i + "\n");
 
         //   Console.In.ReadLine();
         //   for (;;)
         //   {
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
                App.Error?.Log("Bogus: " + Properties.PropBogus);
                App.Info?.Log("Sample Date: " + Properties.SampleDate);
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
                //App.Timer?.ToConsole();
                App.Timer?.Reset();
                Thread.Sleep(1000);
         //   };
        }
    }
}
