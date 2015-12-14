using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleThreadPool myPool = new SimpleThreadPool(4);
            for (int i = 0; i < 60000; i++)
            {
                int tmp = i;
                myPool.Enqueue(() => Work(tmp));
            }
            Console.ReadKey();
            myPool.Dispose();
        }

        static void Work(int i)
        {
            Console.WriteLine("Work {0}", i);
        }
    }
}
