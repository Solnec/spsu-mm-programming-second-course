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
        Queue<Action> QueueTasks = new Queue<Action>();
        ManualResetEvent reset = new ManualResetEvent(false);
        Thread thread;
        int enabled;
        int Index;
       
        public Doer(int i)
        {
            thread = new Thread(delegate() { ThreadFn(); });
            enabled = 1;
            Index = i;
            thread.Start();
        }

        public int GetTaskCount()
        {
            return QueueTasks.Count;
        }

        public bool IsEmpty()
        {
            if (QueueTasks.Count > 0) return false;
            else return true;
        }

        public void AppendEn(Action a)
        {
            QueueTasks.Enqueue(a);
            reset.Set();
        }

        public void Dispose()
        {
           lock(QueueTasks)
           {
               while(QueueTasks.Count > 0)
               {
                   QueueTasks.Dequeue();
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
                if (QueueTasks.Count > 0)
                {
                    Action fn = QueueTasks.Dequeue();
                    Console.WriteLine("Thread[{0}] take an action", Index);
                    fn();
                    Console.WriteLine("Thread[{0}] finished an action", Index);
                }            
                else
                    reset.WaitOne();    
            }
            Console.WriteLine("Thread[{0}] finished his work", Index);
        }
    }
}
