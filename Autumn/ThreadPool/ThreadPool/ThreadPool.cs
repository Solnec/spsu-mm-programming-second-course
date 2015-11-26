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
            getFreeWorker().AppendEn(a);
        }

        private Doer getFreeWorker()
        {
            Doer freeWorker = null;
            int minTasks = Int32.MaxValue;
            foreach (Doer man in workers)
            {
                if (man.IsEmpty()) return man;
                else if (minTasks > man.GetTaskCount())
                {
                    minTasks = man.GetTaskCount();
                    freeWorker = man;
                }
            }
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
    }
}
