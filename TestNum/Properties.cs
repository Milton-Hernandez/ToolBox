using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNum
{
       public interface Addressable : ICloneable
        {
            string AddressIt();
        }

        public class Address : Addressable
        {
            public int Num { get; private set; }
            public string Street { get; private set; }
            public string Town { get; private set; }
            public string Zip { get; private set; }

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
                return (object)new Address(Num, Street, Town, Zip);
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
           // public static IocFactory<Addressable> AddressFactory { get; private set; }

            public enum BogusType
            {
                FIRST,
                SECOND,
                THIRD
            }

            public static DateTime SampleDate { get; set; }

            public static string Prop1 = "DefaultProp1";
            public static string Prop2 = "DefaultProp2";
            public static List<int> Prop3 { get; private set; }

            public static List<int> PropArray = new List<int>();

            public static BogusType PropBogus = BogusType.FIRST;

            public static BogusType[] BogusArray { get; private set; }

            public static Address[] Addresses { get; private set; }

            static Properties() {
              Prop3 = new List<int>();  Prop3.Add(2);
//              PropArray.Add(1);  PropArray.Add(3);
              BogusArray = new BogusType[2];  BogusArray[0] = BogusType.FIRST;  BogusArray[1] = BogusType.SECOND;
              SampleDate = DateTime.Now;  
            }
        }


        public class TestClass
        {
           public  Address[] Addresses { get; private set; }
           public enum BogusType
           {
              FIRST,
              SECOND,
              THIRD
            }

           public DateTime SampleDate { get; private set; }

           public string Prop1 = "DefaultProp1";
           public string Prop2 = "DefaultProp2";

           public Address MainAddress { get; private set; }

           public BogusType BogusProp { get; private set; }

           public List<Address> AddrList { get; private set; }

           public int[] IntArray = { 1, 2, 3, 4, 5 };

           public TestClass()
           {
            SampleDate = DateTime.Now;
            Addresses = new Address[2];
            Addresses[0] = new Address(0,"X","X","X");
            Addresses[1] = new Address(1,"b","c","d");
            BogusProp = BogusType.SECOND;
            MainAddress = new Address(100, "Huckleberry ln", "Ridgefield", "CT");
            AddrList = new List<Address>(Addresses);
           }

         }

    }


