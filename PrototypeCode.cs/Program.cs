using StartKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            /*
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
             */

            int N = 100000;

            var lst = new SearchableList<string>();
            var lst2 = new List<string>();

            for (int i = 0; i < N; i++)
            {
                var val = "Value" + i;
                lst.Add(val);
                lst2.Add(val);
            }


            Stopwatch sp = new Stopwatch();
            sp.Start();
            for (int i = 0; i < lst.Count; i++)
            {
                var x = lst[i];
            }
            Console.Out.WriteLine("My List: " + sp.ElapsedMilliseconds);
            var key = "Value2";
            sp.Restart();
            for (int i = 0; i < N; i++)
            {
                var y = lst[key];
            }
            Console.Out.WriteLine("My Search: " + sp.ElapsedMilliseconds);
            sp.Restart();
            for (int i = 0; i < lst2.Count; i++)
            {
                var y = lst2[i];
            }
            Console.Out.WriteLine("Their List: " + sp.ElapsedMilliseconds);

            Cat.Vals.Data.New("First","Second","Third");
            var res = Cat.Vals.Data.Second;
            Cat.Vals.Orden.New("Primero", "Segundo", "Tercero");
            Console.Out.WriteLine(res);
            Console.Out.WriteLine(Cat.Vals.Orden.Primero);
        }
    }
}
