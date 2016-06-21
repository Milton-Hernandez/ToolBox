using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace StartKit.Serialization
{
    public class AssemblyReflector
    {
        private static IDictionary<string, AssemblyReflector> AssemblyDictionary;

        static AssemblyReflector()
        {
            if (AssemblyDictionary != null)
                return;
            AssemblyDictionary = new Dictionary<string, AssemblyReflector>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(r => !r.IsDynamic).ToArray<Assembly>();
            foreach (var a in assemblies)
            {
                var key = a.GetName().Name;
                if (!AssemblyDictionary.ContainsKey(key))
                {
                    AssemblyDictionary[key.ToUpper()] = new AssemblyReflector(a);
                    Assembly.Load(a.GetName());
                }

            }
        }

        public static void Load()
        {
            //just Triggers static constructor
        }

        public static AssemblyReflector Get(string arg)
        {
            var name = arg.ToUpper();
            if(!AssemblyDictionary.ContainsKey(name))
            {
                return null;
            }
            return AssemblyDictionary[name];
        }

        public static string LoadFromFile(string path)
        {
            var AssemblyToLoad = Assembly.LoadFrom(path);
            string key = AssemblyToLoad.GetName().Name.ToUpper();
            if (!AssemblyDictionary.ContainsKey(key))
                AssemblyDictionary[key] = new AssemblyReflector(AssemblyToLoad);
            return key;
        }

        public static IEnumerable<string> AssemblyNames
        {
          get
            {
                return AssemblyDictionary.Keys;
            }
        }

        private Assembly _assembly;

        private AssemblyReflector(Assembly arg)
        {
            _assembly = arg;
        }

        public IEnumerable<string> TypeNames
        {
          get
            {
                foreach (var t in _assembly.GetTypes())
                    yield return t.FullName;
            }
        }

        public TypeReflector this[string ClassName]
        {
            get
            {
                foreach (var t in _assembly.GetTypes())
                {
                    bool match = t.FullName.ToUpper().Equals(ClassName.ToUpper());
                    if (match)
                        return new TypeReflector(t);
                }
                return null;
            }
        }
    }


    public class TypeReflector {
        private Type _underlyingType;

        private IDictionary<string, FieldInfo> InstanceFieldMap = new Dictionary<String, FieldInfo>();
        private IDictionary<string, PropertyInfo> InstancePropMap = new Dictionary<String, PropertyInfo>();

        private IDictionary<string, FieldInfo> StaticFieldMap = new Dictionary<String, FieldInfo>();
        private IDictionary<string, PropertyInfo> StaticPropMap = new Dictionary<String, PropertyInfo>();

        public Type Value { get { return _underlyingType; } }

        public TypeReflector(Type t) {
            _underlyingType = t;

            var fields = t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields) {
                if (field.IsStatic)
                    StaticFieldMap[field.Name] = field;
                else {
                    InstanceFieldMap[field.Name] = field;
                }
            }

            //Reading Non Public Properties might cause problems
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            foreach (var prop in props)
                StaticPropMap[prop.Name] = prop;

            //Reading Non Public Properties might cause problems
            props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var prop in props)
                InstancePropMap[prop.Name] = prop;
        }

        public IEnumerable<string> Members {
            get {
                var s = new List<string>(StaticFieldMap.Keys);
                s.AddRange(StaticPropMap.Keys);
                return s;
            }
        }

        public IEnumerable<string> FieldNames { get { return StaticFieldMap.Keys; } }
        public IEnumerable<string> PropNames  { get { return StaticPropMap.Keys; } }

        public InstanceReflector New(object[] args = null)
        {
            var types = new Type[] { };
            if(args != null)
             {
                    types = new Type[args.Length];
                    for (int i = 0; i < args.Length; i++)
                        types[i] = args[i].GetType();
              }
            else
             {
                    args = new object[] { };                    
              }

            var Cons = _underlyingType.GetConstructor(types);
            if (Cons == null)
                throw new ApplicationException("Type: " + _underlyingType.Name + " does not have an empty constructor");

             object obj =  Cons.Invoke(args);
             return new InstanceReflector(obj,InstanceFieldMap,InstancePropMap);
        }

        public InstanceReflector Instance(object arg)
        {
            return new InstanceReflector(arg, InstanceFieldMap, InstancePropMap);
        }

        public Type GetType(string name)
        {
            if (StaticFieldMap.ContainsKey(name))
            {
                return StaticFieldMap[name].FieldType;
            }
            else if (StaticPropMap.ContainsKey(name))
            {
                return StaticPropMap[name].PropertyType;
            }
            else
                throw new ApplicationException("Static Field " + name + " not found in class " + _underlyingType.Name);
        }


        public bool AssignableFrom(TypeReflector tr2)
        {
            return _underlyingType.IsAssignableFrom(tr2._underlyingType);
        }

        public override string ToString()
        {
            return "[" + _underlyingType.Name + "]";
        }

        public object this[string name]
        {
            get
            {
                if (StaticFieldMap.ContainsKey(name))
                {
                    return StaticFieldMap[name].GetValue(_underlyingType);
                }
                else if (StaticPropMap.ContainsKey(name))
                {
                    return StaticPropMap[name].GetValue(_underlyingType);
                }
                else
                    throw new ApplicationException("Static Field " + name + " not found in class " + _underlyingType.Name);
            }

            set
            {
                if (StaticFieldMap.ContainsKey(name))
                {
                    StaticFieldMap[name].SetValue(_underlyingType, value);
                }
                else if (StaticPropMap.ContainsKey(name))
                {
                    StaticPropMap[name].SetValue(_underlyingType, value);
                }
                else
                    throw new ApplicationException("Static Field " + name + " not found in class " + _underlyingType.Name);
            }
        }


    }


    public class  InstanceReflector {
        private object _instance;
        private IDictionary<string, FieldInfo> Fields;
        private IDictionary<string, PropertyInfo> Properties;

        public InstanceReflector(object arg, IDictionary<string, FieldInfo> fields, IDictionary<string, PropertyInfo> props)
        {
            _instance = arg;
            Fields = fields;
            Properties = props;
        }

        public Type GetType(string name)
        {
            if (Fields.ContainsKey(name))
            {
                return Fields[name].FieldType;
            }
            else if (Properties.ContainsKey(name))
            {
                return Properties[name].PropertyType;
            }
            else
                throw new ApplicationException("Static Field " + name + " not found in class " + _instance.GetType().Name);
        }

        public object Value {  get { return _instance;  } }

        public IEnumerable<string> FieldNames { get { return Fields.Keys; } }
        public IEnumerable<string> PropNames { get { return Properties.Keys; } }

        public IEnumerable<string> Members
        {
            get
            {
                var s = new List<string>(Fields.Keys);
                s.AddRange(Properties.Keys);
                return s;
            }
        }

        public object this[string name]
        {
            get
            {
                if(Fields.ContainsKey(name))
                {
                    return Fields[name].GetValue(_instance);
                }  
                else if(Properties.ContainsKey(name))
                {
                    return Properties[name].GetValue(_instance);
                }
                else
                    throw new ApplicationException("Instance Field " + name + " not found in class " + _instance.GetType().Name);
            }

            set
            {
                if (Fields.ContainsKey(name))
                {
                    Fields[name].SetValue(_instance, value);
                }
                else if (Properties.ContainsKey(name))
                {
                    Properties[name].SetValue(_instance, value);
                }
                else
                    throw new ApplicationException("Instance Field " + name + " not found in class " + _instance.GetType().Name);
            }
        }
    }
}
