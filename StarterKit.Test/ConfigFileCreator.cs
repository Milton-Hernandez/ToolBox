using StartKit;
using System;
using System.IO;

namespace TestNamespace
{
    public static class ConfigFileCreator
    {
        public static void Create(bool largs = false)
        {
            Runtime.Name = "StartKitTest";

            if (largs)
            {
                string[] args = { "-LL", "WARNING", "-LN", "bcd.log" };
                Runtime.SetArgs(args);
            }

            var tmp = Environment.GetEnvironmentVariable("TMP") + "/" + ".skconf/";
            if (!Directory.Exists(tmp))
                Directory.CreateDirectory(tmp);
            Runtime.ConfigDir = tmp;

            var ConfigStr = @"
                {
                 ### Configuration Files can include any environment variable, like %TMP%, and also they can include a set
                 ### of pre-defined 'special varaibles'.  These are:
                 ###
                 ### %START_TIME%:    Current DateTime at the start of the Program
                 ### %START_DATE%:    Date the program started, with time of 12:00am
                 ### %CONFIG_DIR%:    Location of the Configuration Folder
                 ### %APP_NAME%:      Name of the Runtime
                 ### %EXEC_DIR%:      Execution Folder
                 ### %ASSEMBLY_DIR%:  Folder where the Execution Assembly is located

                  Prop1:'%EXEC_DIR%',
                  ###Known Issue:  Cannot include [ or { in a string, even between quotes 
                  Prop2:'(%1%--%INST_NO%)',

                  ###Secondary files can be included by enclosing them in #FILE#
                  Prop3: #%TMP%\\.skconf\\file.pcfg#,
                  PropArray:  [11,12,13],
                  PropBogus: 'SECOND',
                  BogusArray:   ['SECOND','SECOND','SECOND','FIRST','THIRD' ],
                  SampleDate: '%START_DATE%',
                  Addresses: [
                   {Num: 3, Street: '%START_TIME%', Town: 'Utopia',   Zip: '03603' },
                   {Num: 4, Street:'Calle Segunda', Town: 'Utopia',   Zip: '03603' },
                   {Num: 5, Street:'Calle Tercera', Town: 'Utopia',   Zip: '03603' },
                   {Num: 6, Street:'Calle Cuarta',  Town: '%windir%', Zip: '03603' }
                  ],
                  AddrObject: #%CONFIG_DIR%\SecAddress.pcfg#
                  ### Order: Environment Variabls get evaluated first,  then include files

                }";

            var IncludeStr = @"[1,2,3,4,5,6,7]";

            var IncludeStr2 = @"
                {
                 ClassName$: 'TestLibrary.VirtualAddress',
                 AssemblyName$: 'TestLibrary',
                 AssemblyPath$: [
                        'D:\code\StartKitBak\TestLibrary\bin\Debug\TestLibrary.dll'
                 ],  
                 Memento$:  {
                    XCoord: 1000,
                    YCoord: 2000
                   }   
                }";

            var LogStr = @"
                 {
                  Level: 'DEBUG',
                  LogFolder: 'c:/testapp/logs',
                  LogName: 'sktest.log',
                  LogToConsole: true,
                  LogToFile: false
                }";

            File.WriteAllText(tmp + "/startkit.app.cfg", LogStr);
            File.WriteAllText(tmp + "/testassembly-testnamespace.properties.cfg", ConfigStr);
            File.WriteAllText(tmp + "/file.pcfg", IncludeStr);
            File.WriteAllText(tmp + "/SecAddress.pcfg", IncludeStr2);
               
        }
    }
}
