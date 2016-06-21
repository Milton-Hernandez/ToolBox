using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Logging
{
    public enum LevelType
    {
        ERROR,WARNING,INFO,DEBUG
    }

    [DataContract]
    public class Config
    {
        public static Config Global{ get; set; }

        static string GblLogFolder = Environment.GetEnvironmentVariable("TMP");
        public static string GblAppName = "Default";

        static Config()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var a in assemblies)
            {
                if (a.IsDynamic)
                    continue;
                if (a.Location.ToString().ToLower().Contains(".exe"))
                    GblAppName = a.GetName().Name;
            }

            var HomeValue = Environment.GetEnvironmentVariable(GblAppName.ToUpper() + "_HOME");
            if (HomeValue != null)
                GblLogFolder = HomeValue + System.IO.Path.DirectorySeparatorChar + "logs";
            Global = new Config(); 
        }

        [DataMember] 
        [JsonConverter(typeof(StringEnumConverter))]
        public LevelType Level { get; set; }

        public bool IsDebug { get { return Level == LevelType.DEBUG; } }
    
        [DataMember]
        public string LogFolder  { get; set; }
        [DataMember] 
        public string LogName    { get; set; }
        [DataMember]
        public bool   LogToConsole { get; set; }
        [DataMember]
        public bool   LogToFile { get; set; }
        [DataMember]
        public string ConsoleLayout { get; set; }
        [DataMember]
        public string FileLayout { get; set; }
        [DataMember]
        public string AlertEmailAddress { get; set; }
        [DataMember]
        public string InfoEmailAddress { get; set; }
        [DataMember]
        public string SenderEmailAddress  { get; set; }
        [DataMember]
        public string SMTPHost { get; set; }
        public Config() {
            LogFolder = GblLogFolder;
            LogName = GblAppName + ".log";
            Level = LevelType.INFO;
            LogToConsole = true;
            LogToFile = true;
            ConsoleLayout = @"[${time}]: ${message}";
            FileLayout = @"${longdate}:${threadid} | ${pad:padding=5:inner=${level:uppercase=true}} | ${message}";
            SMTPHost = "osamsmtp";
            AlertEmailAddress = "software_it@osam.com";
            InfoEmailAddress = "software_it@osam.com";
            SenderEmailAddress = "mhernandez@osam.com";
        }
    }
}
