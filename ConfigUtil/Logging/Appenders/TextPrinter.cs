using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit
{
    public abstract class TextPrinter 
    {
        public static readonly ConsoleColor[] DEFAULT = { ConsoleColor.Green };

        public abstract void Write(ConsoleColor color, string str);

        public string Separator { get; set; }

        public TextPrinter(string sep)
        {
            Separator = sep;
        }

        public virtual void Print(ConsoleColor[] colors, params string[] tokens)
        {
            for(int i=0; i<tokens.Length; i++)
            {
                Write(colors[i < colors.Length ? i : colors.Length - 1], tokens[i]);
                if (i < (tokens.Length - 1))
                    Write(ConsoleColor.Gray, Separator);
                else
                    Write(ConsoleColor.White, Environment.NewLine);
            }
        }
    }
}
