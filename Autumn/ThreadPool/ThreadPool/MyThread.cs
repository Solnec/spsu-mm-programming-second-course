//ThreadPool
//Leonova Anna
using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    class MyThread
    {
        Thread thread;
        Action task;
        Queue<Action> queue_tasks;
        Mutex mutex = new Mutex();
        private bool work = true;
        ManualResetEvent happening = new ManualResetEvent(false);

        public MyThread(Queue<Action> queue, int i)
        {
            queue_tasks = queue;
            thread = new Thread(MyStart);
            Console.WriteLine("Created thread {0}", i);
            thread.Start(i);
        }

        public bool Busy
        {
            get 
            {  
                Console.WriteLine("Busy is the thread? -{0}.", happening.WaitOne(0));
                return happening.WaitOne(0); 
            }
        }

        public void Signal()
        {
            Console.WriteLine("I gave the signal.");
            happening.Set();
        }

        public void MyStart(object i) 
        {
            while (work)
            {
                mutex.WaitOne();
                if (queue_tasks.Count > 0)
                {
                    task = queue_tasks.Dequeue();
                    mutex.ReleaseMutex();
                    this.task();
                    Console.WriteLine("The task is completed by thread {0}", i);
                    Console.WriteLine("Queue has {0} tasks", queue_tasks.Count);
                }
                else
                {
                    happening.Reset();
                    happening.WaitOne();
                }
            }

        }

        public void End()
        {
           work = false;
           Signal();
           thread.Join();
        }
    }
}
