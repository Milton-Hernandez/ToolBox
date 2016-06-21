using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit
{
    public class CompositePrinter : TextPrinter
    {
        public readonly static string DefaultSep = " ";

        private List<TextPrinter> Children;

        public CompositePrinter(params TextPrinter[] arg) : base(DefaultSep) {
            Children = new List<TextPrinter>(arg);
        }

        public override void Write(ConsoleColor color, string str)
        {

        }

        public new void Print(ConsoleColor[] colors, params string[] tokens)
        {
            foreach (var c in Children)
                c.Print(colors, tokens);
        }
    }
}
