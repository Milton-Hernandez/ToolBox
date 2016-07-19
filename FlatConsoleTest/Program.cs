using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StartKit;

namespace FlatConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Info?.Log("Huh?");
            App.PrintInfo();
            Console.In.ReadLine();
        }
    }
}
