using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class Doer
    {
        Stack<Action> QueueTasks = new Stack<Action>();
        ManualResetEvent reset = new ManualResetEvent(false);
        Thread thread;
        int enabled;
        public int Index;
        object forLock = new object();

        public Doer(int i)
        {
            thread = new Thread(delegate() { ThreadFn(); });
            enabled = 1;
            Index = i;
            thread.Start();
        }

        public int GetTaskCount()
        {
            lock (forLock)
            {
                return QueueTasks.Count;
            }
        }

        public bool IsEmpty()
        {
            if (Interlocked.Equals(QueueTasks.Count, 0)) return true;
            else return false;
        }

        public void AppendEn(Action a)
        {
            lock (forLock)
            {
                QueueTasks.Push(a);
            }
            reset.Set();
        }

        public void Dispose()
        {
            lock(forLock)
            {
                while (QueueTasks.Count > 0)
                {
                    QueueTasks.Pop();
                }
            }

            Interlocked.Exchange(ref enabled, 0);

            reset.Set();
            thread = null;
        }

        private void ThreadFn()
        {
            while (Interlocked.Equals(enabled, 1))
            {
                if (!Interlocked.Equals(QueueTasks.Count, 0))
                {
                    Action fn = QueueTasks.Peek();
                    Console.WriteLine("Thread[{0}] begin an action", Index);
                    fn();
                    Console.WriteLine("Thread[{0}] finished an action", Index);
                    QueueTasks.Pop();
                }
                else
                {
                    reset.WaitOne();
                }
            }
            Console.WriteLine("Thread[{0}] finished his work", Index);
        }
    }
}
