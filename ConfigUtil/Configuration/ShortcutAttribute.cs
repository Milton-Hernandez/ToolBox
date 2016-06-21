using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit.Configuration
{
   [AttributeUsage(AttributeTargets.All)]
    public class ShortcutAttribute : System.Attribute 
    {
       public string ShortCut { get; private set; }

       public ShortcutAttribute(string arg)
       {
           ShortCut = arg;
       }
    }
}
