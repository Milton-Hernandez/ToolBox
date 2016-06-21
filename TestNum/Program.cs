using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StartKit.Serialization;

namespace TestNum
{
    class Program
    {

        public static char[] op = { '{', '[' };
        public static char[] cl = { '}', ']' };
        public static ParenTokenizer tok1 = new ParenTokenizer(op, cl, ',');
        public static ParenTokenizer tok2 = new ParenTokenizer(op, cl, ':');


        public static void PrintTree(string arg, string prefix)
        {
            var tArg = tok1.UnWrap(arg);

            foreach (var s in tok1.Tokenize(tArg))
            {
                if (s[0] == '\"' && s[s.Length - 1] == '\"')
                    Console.Out.WriteLine(tok1.UnWrap(s));
                else if (s.Contains(":"))
                {
                    var ts = tok2.Tokenize(s).ToList<string>();
                    
                    if (ts.Count() == 1)
                    {
                        if (!ts[0].Contains('{') && !ts[0].Contains('[') && !ts[0].Contains("\"")) 
                            Console.Out.WriteLine(prefix + ts[0]);
                        else
                            PrintTree(ts[0], prefix + " ");
                    }
                    else
                    {
                        Console.Out.Write(prefix + ts[0] + ":");
                        if (!ts[1].Contains('{') && !ts[1].Contains('[') && !ts[1].Contains("\"") )
                            Console.Out.WriteLine(ts[1]);
                        else
                        {
                            Console.Out.Write("\n");
                            PrintTree(ts[1], prefix + " ");
                        }
                    }
                }
                else
                    Console.Out.WriteLine(prefix + s);
            }
        }
 


        static void Main(string[] args)  {

            var d = typeof(Properties).ToDynamic();

            d.Prop1 = "Prop1-Replaced";
            d.Prop2 = "Prop2-Replaced";
            d.BogusArray = new Properties.BogusType[] { Properties.BogusType.SECOND, Properties.BogusType.SECOND, Properties.BogusType.SECOND };


            DynamicHelper.SetStatic(typeof(Properties), d);

            Console.Out.WriteLine(Properties.Prop1);
            Console.Out.WriteLine(Properties.Prop2);
            Console.Out.WriteLine(Properties.BogusArray);
            

            Console.Out.WriteLine("\n\n\n");

            var tc = new TestClass();
            var dyn = tc.ToDynamic();
            dyn.Prop1 = "Changed1";
            dyn.Prop2 = "Changed2";
            dyn.SampleDate = new DateTime(1973, 3, 26);
            Console.Out.WriteLine(DynSerializer.ToJason(dyn));

            var JSON =  "{ Prop1: Changed1,Prop2: Changed2,IntArray:[1, 2, 3, 4, 5],Addresses:[{ Num: 0,Street: X,Town: X,Zip: X},{ Num: 1,Street: b,Town: c,Zip: d}],SampleDate: \"3/26/1973 12:00:00 AM\",MainAddress: { Num: 100,Street: Huckleberry ln, Town:Ridgefield,Zip: CT},BogusProp: SECOND,AddrList:[{ Num: 0,Street: X,Town: X,Zip: X},{ Num: 1,Street: b,Town: c,Zip: d}]}";

            var v = DynamicHelper.FromDynamic((typeof(TestClass)), DynSerializer.Deserialize(JSON));

        }
    }
}
