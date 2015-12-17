using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class MyThreadPool : IDisposable
    {
        private int numberOfThreads;

        private Thread schedulerThread;
        private ManualResetEvent schedulerEvent;

        private Thread[] threads;
        private Dictionary<int, ManualResetEvent> threadsEvent;
        private List<MyTask> listOfTasks = new List<MyTask>();

        private ManualResetEvent stopEvent;
        private bool isStoping;
        private object stopLock;

        private bool isDisposed;

        public MyThreadPool(int number)
        {
            if (number <= 0) Console.WriteLine("It must be 1 or more thread(s).");
            else
            {
                this.numberOfThreads = number;

                this.threads = new Thread[numberOfThreads];

                this.threadsEvent = new Dictionary<int, ManualResetEvent>(numberOfThreads);

                for (int i = 0; i < numberOfThreads; i++)
                {
                    threads[i] = new Thread(ThreadWork) { IsBackground = true };
                    threadsEvent.Add(threads[i].ManagedThreadId, new ManualResetEvent(false));
                    threads[i].Start();
                }

                this.stopLock = new object();
                this.stopEvent = new ManualResetEvent(false);

                this.schedulerEvent = new ManualResetEvent(false);
                this.schedulerThread = new Thread(StartSelectedTask) { IsBackground = true }; ;

                schedulerThread.Start();
            }
        }

        void ThreadWork()
        {
            while (true)
            {
                MyTask task = SelectTask();

                if (task != null)
                {
                    try
                    {
                        task.StartTask();
                    }
                    finally
                    {
                        if (isStoping) stopEvent.Set();
                        threadsEvent[Thread.CurrentThread.ManagedThreadId].Reset();
                    }
                }
            }
        }

        private MyTask SelectTask()
        {
            lock (listOfTasks)
            {
                IEnumerable<MyTask> waitingTasks = listOfTasks.Where(task => !task.IsRunned);

                if (listOfTasks.Count != 0 && waitingTasks.Count() != 0)
                {
                    MyTask selectedTask = waitingTasks.First();
                    listOfTasks.Remove(selectedTask);
                    return selectedTask;
                }
                else return null;
            }
        }

        private void StartSelectedTask()
        {
            while (true)
            {
                schedulerEvent.WaitOne();
                
                lock (threads)
                {
                    for (int i = 0; i < numberOfThreads; i++)
                    {
                        if (!threadsEvent[threads[i].ManagedThreadId].WaitOne(0))
                        {
                            threadsEvent[threads[i].ManagedThreadId].Set();
                            break;
                        }
                        else continue;
                    }
                }

                schedulerEvent.Reset();
            }
        }

        public void AddTask(Action a)
        {
            lock (listOfTasks)
            {
                MyTask newTask = new MyTask(a);
                listOfTasks.Add(newTask);
            }

            schedulerEvent.Set();
        }

        public void Enqueue(Action a)
        {
            lock (stopLock)
            {
                if (isStoping)
                {
                    return;
                }

                AddTask(a);
            }
        }

        public void Stop()
        {
            lock (stopLock)
            {
                isStoping = true;
            }

            while (listOfTasks.Count > 0)
            {
                stopEvent.WaitOne();
                stopEvent.Reset();
            }

            Dispose(true);
        }

        ~MyThreadPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disp)
        {
            if (!isDisposed)
            {
                if (disp)
                {
                    schedulerThread.Abort();
                    schedulerEvent.Dispose();

                    for (int i = 0; i < numberOfThreads; i++) 
                    {
                        threads[i].Abort();
                        threadsEvent[threads[i].ManagedThreadId].Dispose();
                    }
                }
            }
            Console.WriteLine("All threads was disposed");
            isDisposed = true;
        }
    }
}
