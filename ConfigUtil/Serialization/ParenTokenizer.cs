using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit.Serialization
{
    public class ParenTokenizer
    {
        private HashSet<char>  Opener;
        private HashSet<char>  Closer;
        private char Separator;

        private static readonly IDictionary<char, char> Opposites = new Dictionary<char, char>();

        static ParenTokenizer()
        {
            Opposites['['] = ']';
            Opposites['{'] = '}';
            Opposites['('] = ')';
            Opposites['\"'] = '\"';
            Opposites['\''] = '\'';
            Opposites['|'] = '|';
        }

        
        public ParenTokenizer(char op, char close, char sep)
        {
            Opener = new HashSet<char>(); Opener.Add(op);
            Closer = new HashSet<char>(); Closer.Add(close);
            Separator = sep;
        }


        public ParenTokenizer(char[] op, char[] close, char sep)
        {
            Opener = new HashSet<char>(op); 
            Closer = new HashSet<char>(close); 
            Separator = sep;
        }


        public string UnWrap(string arg)
        {
            var tArg = arg.Trim();
            int lst = tArg.Length - 1;

            if (Opener.Contains(tArg[0]))
                if (Closer.Contains(tArg[lst]))
                  if(tArg[lst] == Opposites[tArg[0]] )
                      return tArg.Substring(1, lst-1);

            if (tArg[0] == '\"' && tArg[lst] == '"')
            {
                var ret = tArg.Substring(1, lst - 1);
                if (!ret.Contains('\"'))
                    return ret;
                else
                    throw new IndexOutOfRangeException("Nested Quotes found.  Not allowed");
            }

            return arg;
        }

        public IEnumerable<string> Tokenize(string arg)
        {
            int open = 0;
            int begin = 0;
            int end = 0;
            bool qopen = false;

            foreach(char c in arg) {
                end++;

                if (c == '\"'  || c == '\'')
                {
                    if (qopen)
                        qopen = false;
                    else
                        qopen = true;
                }

                else if (Opener.Contains(c) )
                    open++;
                if (Closer.Contains(c))
                    open--;
                if (open < 0)
                    throw new ApplicationException("Mismatched Parentheses");
                if (c == Separator)
                {
                    if (open == 0 && !qopen)
                    {
                        var len = end - begin - 1;
                        if(len > 0)
                           yield return arg.Substring(begin, len).Trim();
                        begin = end;
                    }
                }
            }
            yield return arg.Substring(begin).Trim();
        }
    }
}
