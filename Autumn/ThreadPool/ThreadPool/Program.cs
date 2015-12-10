using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            bool result = false;
            int CountThreads = 0;

            while (!result)
            {
                Console.WriteLine("Please, enter a number of threads");
                result = Int32.TryParse(Console.ReadLine(), out CountThreads);
            }

            Console.WriteLine("Number of actions is {0}", CountThreads * 5);

            ThreadPool threadPool = new ThreadPool(CountThreads);

            Action someMethod;
            ThreadStart AddAction = delegate()
             {
                 for (int i = 0; i < CountThreads; i++)
                 {
                     someMethod = delegate() { Action(); };
                     threadPool.Enqueue(someMethod);
                 }
             };

            //Adding actions for ThreadPool
            Thread t1 = new Thread(AddAction);
            Thread t2 = new Thread(AddAction);
            Thread t3 = new Thread(AddAction);
            Thread t4 = new Thread(AddAction);
            Thread t5 = new Thread(AddAction);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            Console.ReadKey();

            Console.WriteLine("Begin dispose...");
            threadPool.Dispose();

            Thread.Sleep(500);
            Console.ReadKey();
        }

        static void Action()
        {
            Random r = new Random();
            double item = r.Next(8, 25);
            long sum = 0;
            for (long i = 1; i <= Convert.ToInt64(Math.Pow(2.0, item)); i++)
            {
                sum += i;
            }
        }
    }
}
