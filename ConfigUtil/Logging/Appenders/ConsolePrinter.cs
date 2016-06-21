using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit 
{
    public class ConsolePrinter: TextPrinter
    {
        public ConsolePrinter(string sep = " ") : base(sep) { }
 
        public override void Write(ConsoleColor color, string str)
        {
            Console.ForegroundColor = color;
            Console.Error.Write(str);
        }
    }
}
