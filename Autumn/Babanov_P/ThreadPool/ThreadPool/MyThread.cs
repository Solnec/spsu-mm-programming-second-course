using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace ThreadPool
{
    class MyThread
    {
        bool Enabled; //активность потока
        public Queue<Action> QueueOfMethods;
        Thread MainThread;
        Mutex LockerQueue; //блокировка доступа к очереди
        int index;

        public MyThread (int n)
        {
            Enabled = true;
            QueueOfMethods = new Queue<Action>();
            MainThread = new Thread(ThreadFn);
            MainThread.Start();
            LockerQueue = new Mutex();
            index = n;
        }
        public void AddMethod(Action Fn)
        {
            LockerQueue.WaitOne();
            QueueOfMethods.Enqueue(Fn);
            LockerQueue.ReleaseMutex();
        }
        public bool IsEmpty()
        {
            LockerQueue.WaitOne();
            if (QueueOfMethods.Count == 0)
            {
                LockerQueue.ReleaseMutex();
                return true;
            }
            else
            {
                LockerQueue.ReleaseMutex();
                return false;
            }
        }
        void ThreadFn()
        {
            while (Enabled)
            {
                while(!IsEmpty())
                {
                    LockerQueue.WaitOne();
                    Action tmp = QueueOfMethods.Dequeue();
                    LockerQueue.ReleaseMutex();
                    tmp();
                }
            }
        }
        public void EndWork()
        {
            Enabled = false;
        }
    }
}
