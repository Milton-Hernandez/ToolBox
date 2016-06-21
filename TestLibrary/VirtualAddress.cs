using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNamespace;

namespace TestLibrary
{
    public class VirtualAddress: Addressable
    {
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public string AddressIt() { return String.Format("[{0},{1}]", XCoord, YCoord); }

        public object Clone() { return new VirtualAddress() { XCoord = this.XCoord, YCoord = this.YCoord }; }
    }
}
