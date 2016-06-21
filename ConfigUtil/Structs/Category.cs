using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StartKit
{
    [System.Runtime.InteropServices.StructLayout(LayoutKind.Explicit)]
    public struct Discrete
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        public ushort CatIdx;

        [System.Runtime.InteropServices.FieldOffset(2)]
        public ushort ItemIdx;

        [System.Runtime.InteropServices.FieldOffset(1)]
        public int PayLoad;

        public Discrete(ushort cat, ushort item)
        {
            PayLoad = 0;
            CatIdx = cat;
            ItemIdx = item;
        }

        public override string ToString()
        {
            Category c = Category.ByIndex[CatIdx];
            if (c == null)
                return "NULLCAT.NONE";
            return c.Name + "." + c[ItemIdx];
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Discrete))
                return false;
            return PayLoad.Equals(((Discrete) obj).PayLoad);
        }

        public override int GetHashCode()
        {
            return PayLoad.GetHashCode();
        }
    }

    public class CategoryFinder: DynamicObject
    {
        public static readonly dynamic Cat = new CategoryFinder();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string key = binder.Name;
            result = this[key];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }

        public dynamic this[string key]
        {
            get
            {
                return Category.Get(key);
            }
        }
    }

    public class IndexFinder
    {
        public dynamic this[int idx]
        {
            get
            {
                if (idx >= 0 && idx < Category.CatList.Count)
                    return Category.CatList[idx];
                return null;
            }
        }
    }


    public class Category: DynamicObject {

        internal static IList<Category> CatList = new List<Category>();
        private static IDictionary<string, Category> CatMap = new Dictionary<string, Category>();

        public static readonly IndexFinder ByIndex = new IndexFinder();

        private static object Monitor = new object();

        static Category() {
            CreateCategory("NULLCAT");
        }

        private FlexEnum Items;

        public string Name { get; private set;  }
        private ushort InnerKey = 0;

        private Category(string arg)
        {
            Items = new FlexEnum();
            Name = arg;
            InnerKey = (ushort) CatList.Count;
        }

        private static Category CreateCategory(string name)
        {
            if (CatList.Count >= (ushort.MaxValue - 1))
                throw new ApplicationException("Ran out of Category Indexes");

             var c = new Category(name);
             CatList.Add(c);
             CatMap[c.Name] = c;
            
            return CatMap[name];
        }

        internal static Category Get(string arg)
        {
            Category ret = null;
            lock (Monitor)
            {
                if (CatMap.ContainsKey(arg))
                    ret = CatMap[arg];
                else {
                    ret = CreateCategory(arg);
                }
            }
            return ret;        
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object EnumIdx;
            bool ret = Items.TryGetMember(binder, out EnumIdx);
            result = ToDiscrete(EnumIdx);
            return ret;
        }

        private Discrete ToDiscrete(object arg)
        {
            ushort idx = (ushort)arg;
            if(idx == 0)
                return new Discrete(0, 0);
            return new Discrete(InnerKey, idx);
        }

       
        public Discrete Parse(string arg)
        {
            return ToDiscrete(Items.Parse(arg));
        }

        public string this[ushort arg]
        {
            get
            {
                return Items[arg];
            }
        }

        public Discrete this[string arg]
        {
            get
            {
                return ToDiscrete(Items[arg]);
            }
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }
    }
}
