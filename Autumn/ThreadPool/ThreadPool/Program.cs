using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{   
    public static class Program
    {
        static void Main()
        {
            var pool = new Pool();
            for (var i = 0; i < 20; i++)
            {
                int tmp = i;
                pool.Enqueue(() => Work(tmp));                                                       
            }
            pool.Dispose();
            Console.ReadKey();        
        }

        static void Work(int index)
        {
            Console.WriteLine("Work on {0}", index);
            Thread.Sleep(200);
            Console.WriteLine("Done {0}", index);
        }
    }
}
