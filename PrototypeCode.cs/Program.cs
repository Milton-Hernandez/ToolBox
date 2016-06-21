using StartKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StartKit.App;

namespace PrototypeCode.cs
{
    public class Program
    {
        static void Main(string[] args)
        {
            var p = Cat.Sample;
            var vals = new Discrete[]
            {
                Cat.Sample.ValueOne,
                Cat.Sample.ValueTwo,
                Cat.Sample.ValueThree
            };

            Console.Out.WriteLine(p.TypeName);
            Console.Out.WriteLine(p[1] + "=" + p["ValueOne"]);
            Console.Out.WriteLine(p[2] + "=" + p["ValueTwo"]);
            Console.Out.WriteLine(p[3] + "=" + p["ValueThree"]);

            Console.Out.WriteLine(p[0] + " = NONE?" );
            Console.Out.WriteLine(p[100] + " = NONE?");
            Console.Out.WriteLine(p["NIL"] + " = 0?");
        }
    }
}
