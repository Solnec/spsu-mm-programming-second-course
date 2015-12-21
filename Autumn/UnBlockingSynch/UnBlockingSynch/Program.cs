using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnBlockingSynch
{
    class Program
    {
        static void Main(string[] args)
        {
            bool result = false;
            uint N = 0;
            
            while ((!result) || (N < 1))
            {
                Console.WriteLine("Введите натуральное число");
                result = uint.TryParse(Console.ReadLine(), out N);
            }
            
            Factorial f = new Factorial(N);
            UInt64 Factorial = f.Result();
            Console.WriteLine("{0}! = {1}", N, Factorial);
            Console.ReadKey();
        }
    }
}
