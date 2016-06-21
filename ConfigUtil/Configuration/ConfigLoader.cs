
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using StartKit.Serialization;

namespace StartKit.Configuration
{
    /// <summary>
    /// Config Format Variable Loader</summary>
    /// <remarks>
    /// Allows to load Configuration files in Cfg format
    /// 
    /// ...INSERT CFG GUIDE...
    /// 
    /// Author:   Milton Hernandez.
    /// Created:  March, 2016
    /// </remarks>
    public sealed class ConfigLoader
    {
        #region Public Contract
            /// <summary>Set A Static Configuaration Type</summary>
            /// <remarks>See Cfg Guide</remarks>     
            public static void StaticSet(Type ConfigType, string text)
            {
                var strText = replaceVars(text);
                strText = strText.Replace("\'", "\"");
                dynamic d = DynSerializer.Deserialize(strText);
                DynamicHelper.SetStatic(ConfigType,d);           
             }
        #endregion

        #region Private Parts
            private static IDictionary _envVariables;

            private static string includeFiles(string input)
            {
                var alltoks = AllMatches(input);
                var ret = input;
                foreach (string s in alltoks)
                {
                    var ntok = s.Substring(1, s.Length - 2);
                    try
                    {
                        ret = ret.Replace(s, File.ReadAllText(ntok));
                    }
                    catch (IOException)
                    {
                        throw new ApplicationException("FILE_NOT_FOUND: " + input);
                    } 
                }
                return ret;
            }

            private static string getAssemblyDir()
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            private static string getExecDir()
            {
                return Environment.CurrentDirectory.ToString();
            }

            private static string cleanPath(string arg)
            {
                var value = arg;
                value = value.Replace(@"\\", "||");
                value = value.Replace(@"\", @"\\");
                value = value.Replace(@"||", @"\\");
                return value;
            }

            private static void   populateVars()
            {
                _envVariables = new Dictionary<string, string>();
                var Vars = Environment.GetEnvironmentVariables();
                foreach (var key in Vars.Keys)
                {
                    var k = key.ToString();
                    var v = Vars[k].ToString();
                    _envVariables.Add(k, v);
                }
               foreach (var key in Runtime.Arguments.Keys)
                   _envVariables.Add(key.ToString(), Runtime.Arguments[key.ToString()].ToString());
            }

            private static string replaceVars(string arg)
            {
                if (_envVariables == null)
                {
                    populateVars();
                }
                string ret = arg;
                for (int i = 0; i < 25 && ret.Contains("%") ; i++)
                {
                        foreach (var key in _envVariables.Keys)
                        {
                            if (ret.Contains(key.ToString()))
                            {
                                var value = _envVariables[key.ToString()].ToString();
                                value = cleanPath(value);
                                ret = ret.Replace("%" + key + "%", value);
                            }
                        }
                        if (ret.Contains("%START_TIME%"))
                            ret = ret.Replace("%START_TIME%", DateTime.Now.ToString());
                        else if (ret.Contains("%START_DATE%"))
                        {
                            var now = DateTime.Now;
                            var today = new DateTime(now.Year, now.Month, now.Day);
                            ret = ret.Replace("%START_DATE%", today.ToString());
                        }
                        else if (ret.Contains("%CONFIG_DIR%"))
                            ret = ret.Replace("%CONFIG_DIR%", Runtime.ConfigDir);
                        else if (ret.Contains("%APP_NAME%"))
                            ret = ret.Replace("%APP_NAME%", Runtime.Name);
                        else if (ret.Contains("%EXEC_DIR%"))
                            ret = ret.Replace("%EXEC_DIR%", cleanPath(getExecDir()));
                        else if (ret.Contains("%ASSEMBLY_DIR%"))
                            ret = ret.Replace("%ASSEMBLY_DIR%", cleanPath(getAssemblyDir()));
                        else if (ret.Contains("%INST_NO%"))
                            ret = ret.Replace("%INST_NO%", Runtime.ThisInstance.ToString());

            }

            for (int i = 0; i < 25 && ret.Contains("#") ; i++)
                        ret = includeFiles(ret);
            return ret;
        }
        

        public static string[] AllMatches(string input)
        {
            string pattern = "#([^#]+)#";
            var ret = new List<string>();
            foreach (Match m in Regex.Matches(input, pattern))
                ret.Add(m.Value);
            return ret.ToArray();
        }


        #endregion
    }


}
