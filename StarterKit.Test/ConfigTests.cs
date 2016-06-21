using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StartKit;
using System.IO;

namespace TestNamespace
{
    [TestClass]
    public class ConfigTests
    {
        private string TmpFolder = Environment.GetEnvironmentVariable("TMP") + Runtime.Delim;

        public ConfigTests()
        {
            ConfigFileCreator.Create(true);
        }

        [TestMethod]
        public void a001_ConfigFolderSetCorrectly()
        {
           var tmp = Environment.GetEnvironmentVariable("TMP") + Runtime.Delim + ".skconf" + Runtime.Delim;
           Assert.AreEqual(tmp, Runtime.ConfigDir);
        }

        [TestMethod]
        public void a002_LogPropertiesAreCorrect()
        {
            var tmp = TmpFolder + ".skconf" + Runtime.Delim;
            Assert.AreEqual(true, App.LogToFile);
            Assert.AreEqual(true, App.LogToConsole);
            Assert.AreEqual(TmpFolder + ".StartKitTest"  + Runtime.Delim + "logs", App.LogFolder );
            Assert.AreEqual("StartKitTest.log", App.LogName);
            Assert.AreEqual(LevelType.DEBUG, App.Level);
        }

        [TestMethod]
        public void a003_CustomProp1Correct()
        {
            var exec = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Assert.AreEqual(exec, Properties.Prop1);
        }

        
    }
}
