using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using StartKit.Configuration;
using System.Text.RegularExpressions;
using StartKit.Serialization;

namespace StartKit {
    public static class Settings  {
        #region Members...    

         private static HashSet<string> Processed = new HashSet<string>();
         private static bool Loaded = false;
         public  static List<string> Messages = new List<string>();
         public static List<string> Errors = new List<string>();

         public static IDictionary<string,string> AssembInfo { get; private set; }
        #endregion

        static Settings()
        {
            Load();
        }

        
        private static string UnFormat(string arg)  {
            string ret = "";
            string[] tok = arg.Split('\n');
            foreach (string cur in tok)
                ret += cur.Trim();
            ret = ret.Replace(",}", "}");
            return ret;
        }

        private static string Format(string arg)  {
            if (arg.Trim()[0] == '\"')
                return arg;
            string ret = arg.Replace("[]", "<>")
                .Replace("{", "{\n")
                .Replace("}", "\n}\n")
                .Replace("[", "[\n")
                .Replace("]", "\n]\n")
                .Replace("\n,", ",")
                .Replace(",", ",\n")
                .Replace("\n\n", "\n");

            string[] lines = ret.Split('\n');
            int open = 0;
            string final = "";
            bool split = false;

            for (int i = 0; i < lines.Length; i++)  {

                if (lines[i].Contains('}') || lines[i].Contains(']'))
                    open--;
                for (int j = 0; j < open && !split; j++)
                    final += "   ";
                final += lines[i].Replace("<>", "[]");

                if (!lines[i].Contains(":\""))  {
                    final += '\n';
                    split = false;
                }
                else
                    split = true;
                if (lines[i].Contains('{') || lines[i].Contains('['))
                    open++; 
            }
            return final;
        }

        private static string RemoveComments(string args) {
            string[] lines = args.Split('\n');
            string ret = "";
            foreach (var thisLine in lines)
            {
                var cur = thisLine.Trim();
                if (cur.Length <= 0)
                    continue;
                if (cur[0] != '#')
                    ret += thisLine + "\n";
            }
            return ret;        
        }

        private static void ProcessConfig()  {
            Messages.Add("Found ConfigFile: " + Runtime.ConfigDir);
            foreach (string fileName in Directory.GetFiles(Runtime.ConfigDir))  {
                Messages.Add("Ready to process: " +  fileName);

                if (fileName.ToLower().EndsWith(".cfg"))
                    ProcessConfigFile(fileName);          
            }
            App.WatchProperties = true;
            App.ResetLog();

        }

        public static void Load()   {
                if (Loaded)
                    return;
                Loaded = true;
                LoadAssemblies();
                ProcessConfig();
        }

        private static void LoadAssemblies()  {
            AssemblyReflector.Load();
        }


        private static void ProcessConfigFile(string FileName)  {
            var FName = Path.GetFileName(FileName);
            var Ext = Path.GetExtension(FileName);
            var BaseName = FName.Replace(Ext, " ").Trim();
            string Text = File.ReadAllText(FileName);
            Text = RemoveComments(Text);
            Text = UnFormat(Text);


            if (Ext.Equals(".cfg"))     {

                string[] Tokens = BaseName.Split('-');
                string AssembName = "";
                string ClassName = BaseName;

                if (Tokens.Length == 2)
                {
                    AssembName = Tokens[0];
                    ClassName = Tokens[1];
                }
                if (Tokens.Length == 1)
                {
                    AssembName = "StartKit";
                    ClassName = Tokens[0];
                }

                var assmly = AssemblyReflector.Get(AssembName);
                if (assmly != null) {
                    try {
                        Messages.Add("Assembly Found: " + AssembName);
                        Messages.Add("Class Name: " + ClassName);
                        var tRef = assmly[ClassName];
                        Type ConfigType = tRef.Value;

                        if (ConfigType != null)
                        {
                            ConfigLoader.StaticSet(ConfigType, Text);
                            Messages.Add("Success: " + ClassName);
                            Messages.Add("LogFolder: " + App.LogFolder);
                        }
                        else
                            Messages.Add("MSG Failed to load: " + ClassName);

                        Processed.Add(AssembName);
                    }
                   catch(Exception)
                    {
                        Messages.Add("Exception Loading Class: " + ClassName);
                    }
                }
            }
        }

       internal static object Deserialize(string ClassName, string AssemblyName, string Template)
        {
            var assmly = AssemblyReflector.Get(AssemblyName);
            var tRef = assmly[ClassName];
            Type ConfigType = tRef.Value;
            return DynamicHelper.FromDynamic(ConfigType, DynSerializer.Deserialize(Template.ToString()));
        }

    }   
}
