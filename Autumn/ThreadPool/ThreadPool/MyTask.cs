using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    class MyTask
    {
        private Action work;
        private bool isRunned;

        public MyTask(Action work)
        {
            this.work = work;
        }

        public void StartTask()
        {
            lock (this)
            {
                isRunned = true;
            }
            work();
        }

        public bool IsRunned
        {
            get
            {
                return isRunned;
            }
        }
    }
}
