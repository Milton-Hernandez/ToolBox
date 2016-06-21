using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartKit.Serialization
{
    public static class DynamicHelper
    {
        private const string CLASS_NAME_TAG = "ClassName$";
        private const string ASSEMBLY_NAME_TAG = "AssemblyName$";
        private const string ASSEMBLY_PATH_TAG = "AssemblyPath$";
        private const string MEMENTO_TAG = "Memento$";

        public static object Parse(this Type fieldType, string textValue)
        {
            return ParsePrimitive(fieldType, textValue);
        }

        public static object ParsePrimitive(Type fieldType, string textValue)
        {
            object ret = null;
            try
            {
                string strValue = textValue.ToString().Trim();

                if (fieldType == typeof(bool))
                    ret = bool.Parse(strValue);

                else if (fieldType == typeof(byte))
                    ret = byte.Parse(strValue);

                else if (fieldType == typeof(char))
                    ret = char.Parse(strValue);

                else if (fieldType == typeof(short))
                    ret = short.Parse(strValue);

                else if (fieldType == typeof(ushort))
                    ret = ushort.Parse(strValue);

                else if (fieldType == typeof(int))
                    ret = int.Parse(strValue);

                else if (fieldType == typeof(uint))
                    ret = uint.Parse(strValue);

                else if (fieldType == typeof(long))
                    ret = long.Parse(strValue);

                else if (fieldType == typeof(ulong))
                    ret = ulong.Parse(strValue);

                else if (fieldType == typeof(float))
                    ret = float.Parse(strValue);

                else if (fieldType == typeof(double))
                    ret = double.Parse(strValue);

                else if (fieldType == typeof(DateTime))
                {
                    ret = DateTime.Parse(strValue);
                }
                else if (fieldType.IsEnum)
                {
                    try
                    {
                        ret = Enum.Parse(fieldType, strValue);
                    }
                    catch (ArgumentException)
                    {
                        ret = Enum.Parse(fieldType, strValue.ToUpper());
                    }
                }
                else if (fieldType == typeof(string))
                {
                    ret = strValue;
                }
                return ret;
            }
            catch (Exception)
            {
                throw new ArgumentException("Exception Deserializing " + textValue + " to " + fieldType.FullName);
            }
        }

        public static bool IsPrimitive(this Type fieldType)
        {
                if (fieldType == typeof(bool) ||    fieldType == typeof(byte) ||
                    fieldType == typeof(char) ||    fieldType == typeof(short) ||
                    fieldType == typeof(ushort) ||  fieldType == typeof(int) ||
                    fieldType == typeof(uint) ||    fieldType == typeof(long) ||
                    fieldType == typeof(ulong) ||   fieldType == typeof(float) ||
                    fieldType == typeof(double) ||  fieldType == typeof(DateTime) ||
                    fieldType.IsEnum ||             fieldType == typeof(string) )
                    return true;
            return false;
        }

        public static dynamic ToDynamic(this Type arg)
        {
            if (!(arg.IsAbstract && arg.IsSealed))
                throw new ApplicationException("Attempt to treat a non-static class as static");
            var refl = new TypeReflector(arg);
            var exp = new ExpandoObject() as IDictionary<string, object>;
            foreach(var name in refl.Members)
            {
                var t = refl.GetType(name);
                object o = refl[name];
                if (o == null)
                    continue;
                if (t.IsPrimitive())
                    exp[name] = refl[name].ToString();

                else if (refl[name] is IEnumerable)
                 {
                      var tmp = new List<object>();

                      var en = (IEnumerable) refl[name];
                      foreach(var p in en)
                           tmp.Add(p.ToDynamic());

                    exp[name] = tmp.ToArray();
                }
                else
                    exp[name] = refl[name].ToDynamic();
            }       
            return exp;    
        } 

        public static void SetStatic(Type t, dynamic arg)
        {
            if (!t.IsAbstract || !t.IsSealed)
                throw new ApplicationException("Can not do static parsing in non-static type: " + t.Name);

            var tRef = new TypeReflector(t);
            var argMap = arg as IDictionary<string, object>;

            foreach (var key in tRef.Members)
            {
                var curType = tRef.GetType(key);

                if (argMap.ContainsKey(key) && argMap[key] != null)
                {
                    if (curType.IsPrimitive())
                        tRef[key] = curType.Parse(argMap[key].ToString());
                    else if (tRef[key] is Array)
                    {
                        int len = ((IList)argMap[key]).Count;
                        var y = Array.CreateInstance(curType.GetElementType(), len);
                        int i = 0;
                        foreach (var o in ((IList)argMap[key]))
                        {
                            y.SetValue(FromDynamic(curType.GetElementType(), o), i);
                            i++;
                        }
                        tRef[key] = y;
                    }
                    else if (tRef.GetType(key).Name.Equals("List`1"))
                    {
                        var src = (IEnumerable)argMap[key];
                        var lst = (IList)Activator.CreateInstance(curType);

                        foreach (var obj in src)
                            lst.Add(FromDynamic(curType.GenericTypeArguments[0], obj));
                        tRef[key] = lst;

                    }
                    else {
                        tRef[key] = FromDynamic(curType, argMap[key]);
                    }
                }
            }
        }

        public static dynamic ToDynamic(this object obj)
        {
            Type t = obj.GetType();
            if (t.IsPrimitive())
                return obj.ToString();
            var refl = new TypeReflector(obj.GetType()).Instance(obj);
            var exp = new ExpandoObject() as IDictionary<string, object>;
            foreach (var name in refl.Members)
            {
                object o = refl[name];
                if (o == null)
                    continue;
                if (o.GetType().IsPrimitive())
                    exp[name] = o;
                else if (refl[name] is IEnumerable)
                {
                    var tmp = new List<object>();

                    var en = (IEnumerable)refl[name];
                    foreach (var p in en)
                        tmp.Add(p.ToDynamic());
                    exp[name] = tmp.ToArray();
                }
                else
                    exp[name] = refl[name].ToDynamic();
            }       
            return exp;
        }

        public static object FromDynamic(Type t, dynamic arg)
        {
            var tRef  = new TypeReflector(t);
            var map = arg as IDictionary<string, object>;
            if (t.IsPrimitive())
                return ParsePrimitive(t, arg.ToString());

            else if (t.Name.Contains("[]") )
            {
                int len = arg.Count;
                var y = Array.CreateInstance(t.GetElementType(), len);
                int i = 0;
                foreach (var o in arg)
                {
                    y.SetValue(FromDynamic(t.GetElementType(), o), i);
                    i++;
                }
                return  y;
            }
            else if (t.Name.Equals("List'1") )
            {
                var src = (IEnumerable)arg;
                var lst = (IList)Activator.CreateInstance(t);

                foreach (var obj in src)
                    lst.Add(FromDynamic(t.GenericTypeArguments[0], obj));
                return lst;

            }
            else if (map.ContainsKey(CLASS_NAME_TAG) )
            {
                var aRef = AssemblyReflector.Get(map[ASSEMBLY_NAME_TAG].ToString());
                if(aRef == null)
                {
                    if (map.ContainsKey(ASSEMBLY_PATH_TAG))
                    {
                        foreach (var line in (IEnumerable)map[ASSEMBLY_PATH_TAG])
                        {
                            AssemblyReflector.LoadFromFile(line.ToString());
                        }
                    }
                    else
                        throw new ApplicationException("Injected Assembly " + map[ASSEMBLY_NAME_TAG].ToString() + " has not being loaded. Please add the Assembly dll and its dependencies to the $AssemblyPath list");
                    aRef = AssemblyReflector.Get(map[ASSEMBLY_NAME_TAG].ToString());
                }

                var cname = map[CLASS_NAME_TAG].ToString();

                var InjectedType = aRef[cname];
                if(InjectedType == null)
                {
                    throw new ApplicationException("Type " + cname + " not found in Assembly " + map[ASSEMBLY_NAME_TAG] + ".  Make sure you are using the fully qualified class name");
                }

                if (!tRef.AssignableFrom(InjectedType))
                    throw new ApplicationException("Type " + tRef + " cannot be assigned Type " + InjectedType);

                if (map.ContainsKey(MEMENTO_TAG))
                    return FromDynamic(InjectedType.Value, map[MEMENTO_TAG]);
                else
                    return InjectedType.New();
            }
           
            var obRef = tRef.New();

            var argMap = arg as IDictionary<string, object>;

            foreach(var key in obRef.Members)
            {
                var curType = obRef.GetType(key);
                if (argMap.ContainsKey(key) && argMap[key] != null) {
                    if (curType.IsPrimitive())
                        obRef[key] = curType.Parse(argMap[key].ToString());
                    else if(obRef[key] is Array )
                    {
                       int len = ((IList) argMap[key]).Count;
                       var y = Array.CreateInstance(curType.GetElementType(), len);
                       int i = 0;
                       foreach (var o in ((IList)argMap[key])) {
                            y.SetValue(FromDynamic(curType.GetElementType(), o), i);
                            i++;
                       }
                        obRef[key] = y;
                    }
                    else if(obRef[key] is IList)
                    {
                        var src = (IEnumerable)argMap[key];
                        var lst = (IList) Activator.CreateInstance(curType);

                        foreach (var obj in src)
                            lst.Add(FromDynamic(curType.GenericTypeArguments[0],obj));
                        obRef[key] = lst;

                    } 
                    else
                        obRef[key] = FromDynamic(curType, argMap[key]);
                }
            }
            return obRef.Value;
        }
    }
}
