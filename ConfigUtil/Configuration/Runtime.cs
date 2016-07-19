using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ToolBox
{
    /// <summary>
    /// Repository for Runtime Level Settings</summary>
    /// <remarks>
    /// This class is meant to summarize and simplify 90% of  the settings that 
    /// are common to all applications developed in C#, with an eye towards 
    /// Convention + Configuration, in other words,  things work out of the box
    /// by Converntion,  but they can be changed by configuration.
    /// 
    /// Author:   Milton Hernandez.
    /// Created:  March, 2016
    /// </remarks>
    public class Runtime
    {
        #region Public Contract
        /// <summary>Read-only storage for Command-Line Arguments</summary>
        /// <remarks>The Dictionary can be accessed by using a key.  This allows for
        /// the use of named arguments in the form: -Arg1 Value1 -Arg2 Value2 ... -NameN ValueN.
        /// In that case the values starting with '-' become the key for the argument immediately
        /// after it (not including the "-".  For the example above, 
        /// Appication.Arguments["Arg1"] would return "Value1".  If, on the other hand, you want
        /// to use unnamed arguments, just use the 1-based number of the argument.  In the preceding
        /// example, Runtime.Arguments["1"] would return "-Arg1", and Runtime.Arguments["2"] would
        /// return "Value1".  The property is read-only.  To set the arguments, 
        /// see the method SetArgs(string args[]).</remarks>
        public static Dictionary<string, string> Arguments { get { return _arg; } private set { _arg = value; }  }

        /// <summary>Number of the Current Process Instance</summary>
        /// 
        /// <remarks>This facility is meant to be used by Executable Applications, when they are running
        /// Several Instances of the Process.  If MaxInstances is set to anything higher than Zero, 
        /// This instance will automatically hold the number of the current instance of the process, in 
        /// the order in which they were started.</remarks>
        public static int ThisInstance
        {
            get {  return _thisInstance;  }

            private set { _thisInstance = value; }
        }

        /// <summary>Maximum Allowed Number of Concurrent Process Instances for the App</summary>
        /// 
        /// <remarks>Set this Value before anything else in your Main method.  Then execute multimple processes
        /// normally.  When this property is set to N,  the N+1 process initiated will fail.  The value ThisInstance
        /// reflects the number of the current instance.  If there are, e.g. 8 instances running in the same machine,
        /// and the instance #6 stops,  the next executed instance will take that value, rather than the N+1</remarks>
        public static int MaxInstances
        {
            get
            {
                return _maxInstances;
            }

            set
            {
                _maxInstances = (int)value;
                if (_maxInstances > 0)
                {
                    ThisInstance = getNextFreeInstNumber();
                }
            }
        }

        /// <summary>
        /// (Read-only) Path of the Executing Assembly
        /// </summary>
        public static string AssemblyPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>Runtime Name</summary>
        /// <remarks>Defaults to the name of the executing assembly if not set
        /// Needs to be set at the beggining of Main Method</remarks>
        public static string Name
        {
            get
            {
                if (_name == null)
                    _name = Assembly.GetEntryAssembly().GetName().Name;
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>Path for the Configuration Files to be loaded</summary>
        /// <remarks>If not set, it defaults to the Assembly Path.  Load in the first few lines of the Main
        /// Method</remarks>
        public static string ConfigDir
        {
            get
            {
                if (_config == null)
                {
                    _config = AssemblyPath;
                }
                return _config;
            }

            set
            {
                _config = value;
            }
        }

        /// <summary>Path of the Output folder.  Including Logging (Logging can be set separately).</summary>
        /// <remarks>Defaults to the folder indicated by the TMP enviroment variable</remarks>
        public static string OutputDir
        {
            get
            {
                if (_output == null)
                {
                    _output = getOrCreateOutputFolder();
                }
                return _output;
            }

            set
            {
                _output = value;
            }
        }

        /// <summary>
        /// Sets the Values of the Command Line Arguments</summary>
        /// 
        /// <remarks>This method sets the values for the "Arguments" Dictionary. 
        /// This allows for the use of named arguments in the form: -Arg1 Value1 -Arg2 Value2 ... -NameN ValueN.
        /// In that case, the values starting with '-' become the key for the argument immediately
        /// after it (not including the "-".  For the example above, 
        /// Appication.Arguments["Arg1"] would return "Value1".  If, on the other hand, you want
        /// to use unnamed arguments, just use the 1-based number of the argument.  In the preceding
        /// example, Runtime.Arguments["1"] would return "-Arg1", and Runtime.Arguments["2"] would
        /// return "Value1".  IMPORTANT:  Make this your first call in the main method.  </remarks>
        public static void SetArgs(string[] args)
        {
            Arguments = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                var key = "" + (i + 1);
                Arguments.Add(key, args[i]);
                if (args[i][0] == '-' && i < args.Length - 1)
                    Arguments.Add(args[i].Substring(1), args[i + 1]);
            }
        }

        static Runtime()
        {
            ThisInstance = -1;
        }

        public static readonly char Delim = System.IO.Path.DirectorySeparatorChar;
        #endregion

        #region Private Parts :-)
        private static string _name;               //Common Name for the Runtime      
        private static string _config;             //Location of the Configuration Folder
        private static string _output;             //Location of the Output folder
        private static int _maxInstances = 0;      //Maximum Concurrent Instances of the Process allowed
        private static int _thisInstance = 0;      //Process number for the current instance
        private static Dictionary<string, string> _arg = new Dictionary<string, string>();
        private static FileStream _fsLock;         //Lock in the FileSystem to "Claim" a process number 

        private static string getOrCreateOutputFolder()
        {
            var tmp = Environment.GetEnvironmentVariable("TMP");
            var path = Path.Combine(tmp, "." + Runtime.Name);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }

        private static int getNextFreeInstNumber()
        {
            int inst = 0;
            for (; inst < MaxInstances; inst++)
            {
                string path = Path.Combine(OutputDir, ".lock_" + inst);
                try
                {
                    _fsLock = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    break;
                }
                catch (IOException)
                {
                    continue;
                }
            }
            if (inst >= MaxInstances)
                throw new ApplicationException("Maximum Number of Instances Exceeded.  You can extend the number by setting Runtime.MaxInstances.");
            return inst;
        }
        #endregion
    }
}
