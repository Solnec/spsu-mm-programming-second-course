using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class ThreadPool : IDisposable
    {
        Doer[] workers;
        bool disposed = false;
        object thisLock = new object();

        public ThreadPool(int Count)
        {
            workers = new Doer[Count];
            for (int i = 0; i < Count; i++)
            {
                workers[i] = new Doer(i);
            }
        }

        public void Enqueue(Action a)
        {
            lock (thisLock)
            {
                GetFreeWorker().AppendEn(a);
            }
        }

        private Doer GetFreeWorker()
        {
            Doer freeWorker = null;
            int minTasks = Int32.MaxValue;
            Console.WriteLine("MainThread is searching free Thread");

            foreach (Doer man in workers)
            {
                if (man.IsEmpty())
                {
                    Console.WriteLine("MainThread has found empty thread. Its index = [{0}]", man.Index);
                    return man;
                }
                else if (minTasks > man.GetTaskCount())
                {
                    minTasks = man.GetTaskCount();
                    freeWorker = man;
                }
            }

            Console.WriteLine("MainThread has found one of the most free. It has {0} tasks in Stack. Its index = [{1}]", freeWorker.GetTaskCount(), freeWorker.Index);
            return freeWorker;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                foreach (Doer man in workers)
                {
                    man.Dispose();
                }
            }
            disposed = true;
        }

        ~ThreadPool()
        {
            Dispose(false);
        }
    }
}
