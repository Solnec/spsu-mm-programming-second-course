using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    public sealed class Pool : IDisposable
    {
        private LinkedList<Thread> _workers;
        private Queue<Action> _tasks = new Queue<Action>();       
        bool _disposed; 
        bool AllWorkDone = false; 
        int size = 7;

        public Pool()
        {
            this._workers = new LinkedList<Thread>();
            for (var i = 0; i < size; ++i)
            {
                var worker = new Thread(this.Worker);
                worker.Start();
                this._workers.AddLast(worker);
            }
        }
        
        public void Dispose()
        {       
            if (AllWorkDone == false)
            {
                lock (this._tasks)
                {
                    if (this._disposed == false)
                    {
                        while (this._tasks.Count > 0)
                        {
                            Monitor.Wait(this._tasks);
                        }
                        this._disposed = true;
                        Monitor.PulseAll(this._tasks);
                        AllWorkDone = true;
                    }
                }             
            }

            if (AllWorkDone == true)
            {
                foreach (var worker in this._workers)
                {
                    worker.Join();
                }
                _workers.Clear();
            }
        }

        public void Enqueue(Action task)
        {
            lock (this._tasks)
            {           
                this._tasks.Enqueue(task);
                Monitor.PulseAll(this._tasks);
            }
        }

        private void Worker()
        {
            Action task = null;
            while (true)
            {
                lock (this._tasks)
                {
                    while (true)
                    {
                        if (this._disposed)
                        {
                            return;
                        }
                        if (null != this._workers.First && this._tasks.Count > 0 && this.Check) //Можем продолжить только если поток первый в списке и есть доступная задача
                        {
                            task = this._tasks.Dequeue();
                            this._workers.RemoveFirst();
                            Monitor.PulseAll(this._tasks); 
                            break; 
                        }
                        Monitor.Wait(this._tasks); 
                    }
                }

                task();
                lock (this._workers)
                {
                    this._workers.AddLast(Thread.CurrentThread);
                    Monitor.PulseAll(this._workers);
                }
            }
        }
        public bool Check
        {
            get
            {
                return object.ReferenceEquals(Thread.CurrentThread, this._workers.First.Value);
            }
        }
    
    }
}