using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigUtil;
using Logging;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace ConfigTest {
  
    class Program  {

        static void TestEmail()  {
            Logging.Config.Global.InfoEmailAddress = "mhernandez@osam.com";
            Logging.Config.Global.AlertEmailAddress = "bill.reynolds@osam.com";
            Logging.Config.Global.SenderEmailAddress = "mhernandez@osam.com";
            Email.Inform("Informational Test Email","This is a Test\nDo Not Panic.\nOK. You can panic a little\n");
            Email.Alert("Informational Test Email","This is a Test\nDo Not Panic.\nOK. You can panic a little\n");
        }

        static void TestProfiler(int size) {
            for (int c=0;; c++) {
                Logging.Timer.Reset();
                Random r = new Random();
                int[] t = new int[size];
                for (int i = 0; i < size; i++)
                    t[i] = r.Next() % 300;
                Logging.Timer.Reset();
                Parallel.For(0, 10, index => {
                    for (int i = 0; i < size; i++)  {
                        Logging.Timer.CheckPoint("Stage" + (i + 1));
                        Thread.Sleep(t[i]);
                    }
                });
                var Stats = Logging.Timer.Stats();
                Logging.Timer.ToConsole("Title Test", Stats);
            }               
        }

        static void Main(string[] args)  {
            try  {
                Config.Settings.Load();
                int size = 10;
                TestProfiler(size);
             }
            catch (Exception ex) {
                Log.Error("Problems.", ex);
            }
        }  
    }
}
