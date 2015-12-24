using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            MyThreadPool Pool = new MyThreadPool(4);
            for(int i = 1; i < 10; i++)
            {
                Pool.Add(DoerSomething);
            }
            Console.ReadLine();
            Pool.Dispose();
        }

        static void DoerSomething()
        {
            for(int i = 1; i<100; i++)
            {
                Console.WriteLine(i.ToString());
            }
        }
    }
}
