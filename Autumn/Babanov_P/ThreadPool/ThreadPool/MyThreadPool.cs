using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    class MyThreadPool:IDisposable
    {
        delegate void FunctionOfEndWork();
        event FunctionOfEndWork FinishingWork;
        int AmountOfThread;
        List<MyThread> Threads;
        
        public MyThreadPool (int n)
        {
            AmountOfThread = n;
            Threads = new List<MyThread>();
            for (int i = 0; i < n; i++)
            {
                Threads.Add(new MyThread(i));
                FinishingWork += Threads[i].EndWork;
            }
        }

        public void Add(Action NewTask)
        {
            int min = Threads[0].QueueOfMethods.Count;
            int minEl = 0;
            foreach(MyThread n in Threads)
            {
                if (n.QueueOfMethods.Count<min)
                {
                    min = n.QueueOfMethods.Count;
                    minEl = Threads.IndexOf(n);
                }
            }
            Threads[minEl].QueueOfMethods.Enqueue(NewTask);
        }
        public void Dispose()
        {
            FinishingWork();
        }
    }
}
