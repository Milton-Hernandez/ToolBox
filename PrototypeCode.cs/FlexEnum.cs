using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeCode.cs
{
    [System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
    public struct Discrete
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        private int Category;

        [System.Runtime.InteropServices.FieldOffset(4)]
        private int Item;

        [System.Runtime.InteropServices.FieldOffset(0)]
        private ulong LongValue;

        public Discrete(int cat, int item)
        {
            LongValue = 0;
            Category = cat;
            Item = item;
        }

        public override string ToString()
        {
            string name = FlexEnum.Names[Category];
            if (name == null)
                return "Null.None";
            return Cat.Map[name].ToName(Item);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Discrete))
                return false;
            return LongValue.Equals(((Discrete)obj).LongValue);
        }

        public override int GetHashCode()
        {
            return LongValue.GetHashCode();
        }
    }


    public class Cat: DynamicObject {

        public readonly static Dictionary<string, FlexEnum> Map = new Dictionary<string, FlexEnum>();

        public readonly static dynamic Vals = new Cat();

        public string TypeName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var key = binder.Name;
            result = FlexEnum.Init(key);
            return true;
        }
       
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }

    }


      public class FlexEnum : DynamicObject
        {
            public readonly static SearchableList<string> Names = new SearchableList<string>();
            private HashSet<object> Fields = new HashSet<object>();
            private string Name;
            private int MyIdx;

            private FlexEnum(String name)
            {
              Name = name;
              Names.Add(Name);
              MyIdx = Names[Name];
            }

            public static FlexEnum Init(string Name)
            {
              FlexEnum FEnum;
              if (Cat.Map.ContainsKey(Name))
                FEnum = Cat.Map[Name];
              else 
                FEnum = new FlexEnum(Name);
              Cat.Map[Name] = FEnum;
              return FEnum;
            }
            
            public void New(params string[] arg)
            {
             foreach (var k in arg)
             {
                Names.Add(k);
                Fields.Add(Names[k]);
             }
            }

            public string TypeName
            {
                get
                {
                    return this.GetType().Name;
                }
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {               
                var key = binder.Name;
                var item = Names[key];
                result = new Discrete(MyIdx, item);
                if ( Fields.Contains(item) )
                  return true;
                else 
                  return false;
            }

            public int Parse(string arg)
            {
              var result = Names[arg];
              if (Fields.Contains(result))
                return result;
              return -1;
            }

            public string ToName(int i)
            {
              if (Fields.Contains(i))
                return Name + "." + Names[i];
               return Name + ".Null";
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                return false;
            }
        }
    }
