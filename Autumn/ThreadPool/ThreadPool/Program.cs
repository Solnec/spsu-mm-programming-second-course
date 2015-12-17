using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Program
    {
        static void Main()
        {
            int number = Int32.Parse(Console.ReadLine());
            var a = new MyThreadPool(number);

            ThreadStart TS = delegate
            {
                a.Enqueue(() => Console.WriteLine("{0} written by thread #{1}",new Random().Next(0, 64000), Thread.CurrentThread.ManagedThreadId));
            };

            Thread[] threads = new Thread[5 * number];

            for (int i = 0; i < 5 * number; i++)
            {
                threads[i] = new Thread(TS);
                threads[i].Start();
            }

            Console.ReadLine();

            a.Dispose();

            Console.ReadLine();
        }
    }
}
