//ThreadPool
//Leonova Anna
using System;
using System.Threading;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            MyThreadPool ThreadPool = new MyThreadPool();
            Action task = action;
            for (int i = 1; i < 11; i++)
            {
                Console.WriteLine();
                ThreadPool.AddTasks(task, i);
            }
            Thread.Sleep(1500);
            ThreadPool.Dispose();
            Console.ReadLine();
        }

        static void action()
        {
            Console.WriteLine();
            Console.WriteLine("*** I task. The task is done ...");
            Thread.Sleep(500);
            Console.WriteLine();
        }
    }

   
}
