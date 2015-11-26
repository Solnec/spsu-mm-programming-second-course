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
            Action someMethod;

            while (!result)
            {
                Console.WriteLine("Please, enter a number of threads");
                result = Int32.TryParse(Console.ReadLine(), out CountThreads);
            }

            Console.WriteLine("Number of actions is {0}", CountThreads * 3);

            ThreadPool threadPool = new ThreadPool(CountThreads);

            int i = 1;

            //Adding actions for ThreadPool
            do
            {
                int Index = i;
                someMethod = delegate() { Action(Index); };
                threadPool.Enqueue(someMethod);
                i++;
            } while (i <= 3 * CountThreads);

            Console.ReadKey();

            Console.WriteLine("Begin dispose...");
            threadPool.Dispose();

            Thread.Sleep(500);
            Console.ReadKey();
        }

        static void Action(int A)
        {
            Random r = new Random();
            double item = r.Next(8, 25);

            Console.WriteLine("Action[{0}] is added", A);
            long sum = 0;
            for (long i = 1; i <= Convert.ToInt64(Math.Pow(2.0, item)); i++)
            {
                sum += i;
            }
            Console.WriteLine("Action[{0}] is finished. Result = {1}", A, sum);
        }
    }
}
