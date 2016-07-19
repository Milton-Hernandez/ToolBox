using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolBox;

namespace TestNamespace
{
    public interface Addressable : ICloneable
    {
       string AddressIt();
    }

    public class Address: Addressable
    {
        public int Num { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Zip { get;  set; }

        public Address() { }

        public Address(int n, string s, string t, string z)
        {
            Num = n;
            Street = s;
            Town = t;
            Zip = z;
        }

        public object Clone()
        {
            return (object) new Address(Num,Street,Town,Zip);
        }

        public string AddressIt()
        {
            return ToString();
        }

        public override string ToString()
        {
            return String.Format("{0} {1}, {2}, {3}", Num, Street, Town, Zip);
        }
    }

    public static class Properties
    {
        public enum BogusType
        {
            FIRST,
            SECOND,
            THIRD
        }

        public static Addressable AddrObject { get; set; }

        public static DateTime SampleDate { get; set; }

        public static string Prop1 = "DefaultProp1";
        public static string Prop2 = "DefaultProp2";
        public static List<int> Prop3 { get; private set; }

        public static List<int> PropArray = new List<int>();

        public static BogusType PropBogus = BogusType.FIRST;

        public static BogusType[] BogusArray { get; private set; }

        public static Address[] Addresses { get; private set; }

    }

}

