using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonBlockingFactorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Factorial.Fact(7));
            Console.WriteLine(Factorial.Fact(55));
            Console.ReadKey();
        }

    }
}
