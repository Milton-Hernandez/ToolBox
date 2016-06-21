using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit
{
    /// <summary>Inversion of Control Base Class</summary>
    /// <remarks>
    /// Starkit includes a basic Inversion-of-Control  Framework
    /// Which allows to Inject new classes into a compiled app
    /// 
    /// Author:   Milton Hernandez.
    /// Created:  March, 2016
    /// </remarks>
    public class IocFactory<T> where T: ICloneable
    {
        public string ClassName { get; set; }    //Fully qualified name of the Class to be instantiated
        public string FileName { get; set; }     //Name of the DLL to be loaded  (Optional)
        public string FilePath { get; set; }     //Path of the DLL (Optional: Defaults to Code Base folder)
        public T Template { get; set; }     //Properties of the class that need to be Set

        /// <summary>Create a new Instance of the Class (Don't use NEW)</summary>
        /// <remarks>See Cfg Guide</remarks>     
        public T NewInstance()
        {
            return (T) Settings.Deserialize(ClassName, FileName, Template.ToString());
        }

        /// <summary>Printable String infomation for Object Factory</summary>
        public override string ToString()
        {
            string ret =  String.Format("ClassName: {0}.  FileName: {1}.  FilePath: {2}.  Template: {3}",
                                         ClassName, FileName, FilePath, Template);
            return ret;
        }
    }

}
