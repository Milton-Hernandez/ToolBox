using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit.Serialization
{
    public static class DynSerializer
    {
        private static char[] op = { '{', '[' };
        private static char[] cl = { '}', ']' };
        private static ParenTokenizer tok1 = new ParenTokenizer(op, cl, ',');
        private static ParenTokenizer tok2 = new ParenTokenizer(op, cl, ':');

        private static void ParseTree(string arg, dynamic parent)
        {
            try
            {
                var tArg = tok1.UnWrap(arg);

                var map = parent as IDictionary<string, object>;

                foreach (var s in tok1.Tokenize(tArg))
                {
                    if (s[0] == '\"' && s[s.Length - 1] == '\"')
                        parent.Add(tok1.UnWrap(s));
                    else if (s.Contains(":"))
                    {
                        var ts = tok2.Tokenize(s).ToList<string>();

                        if (ts.Count() == 1)
                        {
                            if (!ts[0].Contains('{') && !ts[0].Contains('['))
                                parent.Add(ts[0]);
                            else
                            {
                                if (ts[0][0] == '{')
                                {
                                    var o = new ExpandoObject();
                                    parent.Add(o);
                                    ParseTree(ts[0], o);
                                }
                                else if (ts[0][0] == '[')
                                {
                                    ParseTree(ts[0], parent);

                                }

                            }
                        }
                        else
                        {
                            if (!ts[1].Contains('{') && !ts[1].Contains('['))
                                map[ts[0]] = tok1.UnWrap(ts[1]);
                            else
                            {
                                if (ts[1][0] == '{')
                                {
                                    var o = new ExpandoObject();
                                    map[ts[0]] = o;
                                    ParseTree(ts[1], o);
                                }
                                else if (ts[1][0] == '[')
                                {
                                    var l = new List<dynamic>();
                                    map[ts[0]] = l;
                                    ParseTree(ts[1], l);
                                }

                            }
                        }
                    }
                    else {
                        parent.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                var ne = new ApplicationException("Exception while Parsing String: " + arg, ex);
                throw ne;
            }
        }

        public static dynamic Deserialize(string args)
        {
            var ret = new ExpandoObject();
            ParseTree(args,ret);
            return ret;
        }

        public static string ToJason(dynamic arg)
        {
            StringBuilder buf = new StringBuilder();
            try
            {
                if (arg is ExpandoObject)
                {
                    var map = (IDictionary<string, object>)arg;
                    buf.Append('{');
                    var keys = map.Keys;
                    int cnt = 0;
                    foreach (var key in map.Keys)
                    {
                        cnt++;
                        buf.Append(key).Append(':').Append(ToJason(map[key])).Append(cnt >= keys.Count ? '}' : ',');
                    }
                }
                else if (arg is Array)
                {
                    var lst = (Array)arg;
                    buf.Append('[');
                    int cnt = 0;
                    foreach (var x in lst)
                    {
                        cnt++;
                        buf.Append(ToJason(x)).Append(cnt >= lst.GetLength(0) ? ']' : ',');
                    }
                    if (lst.GetLength(0) == 0)
                        buf.Append(']');
                }
                else
                {
                    var str = arg.ToString();
                    if ((str.Contains(":") || str.Contains(",")) && !str.Contains("\""))
                        buf.Append('\"').Append(str).Append('\"');
                    else
                        buf.Append(str);
                }
                return buf.ToString();
            }
            catch (Exception ex)
            {
                var ne = new ApplicationException("Exception while Serializing Object (" + buf.ToString() + ")",ex);
                throw ne;
            }
        }



    }
}
