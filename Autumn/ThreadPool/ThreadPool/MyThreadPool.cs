//ThreadPool
//Leonova Anna
using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    class MyThreadPool : IDisposable
    {
        private const int count = 4;
        private static MyThread[] threads;
        Queue<Action> queue_tasks = new Queue<Action>();

        public MyThreadPool()
        {
            threads = new MyThread[count];
            for (int i = 0; i < count; i++) threads[i] = new MyThread(queue_tasks, i+1);
        }

        public void AddTasks(Action task, int num)
        {
            Monitor.Enter(task);
            queue_tasks.Enqueue(task);
            Console.WriteLine("The task {0} added to the queue.", num);
             for (int i = 0; i < count; i++)
             {
                 if (threads[i].Busy == false)
                 {
                     Console.WriteLine("The task №{0} given to the thread {1}.", num, i);
                     threads[i].Signal(); 
                     break;
                 }
             }
             Monitor.Exit(task);
             Console.WriteLine();
        }

        public void Dispose()
        {
            Console.WriteLine();
            for (int i = 0; i < count; i++)
            {
                threads[i].End();
                Console.WriteLine("The thread {0} finished working.", i);
            }
            Console.WriteLine();
            Console.WriteLine("All threads finished working.");
        }

    }
}
