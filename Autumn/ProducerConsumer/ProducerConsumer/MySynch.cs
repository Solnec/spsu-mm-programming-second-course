using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class MySynch
    {
        private int IsResourseUsingNow = 1;

        public void Set()
        {
            Interlocked.Exchange(ref IsResourseUsingNow, 0);
        }

        public void TryEnter()
        {
            while (Interlocked.Equals(IsResourseUsingNow, 1))
                Thread.Sleep(1);

            Interlocked.Exchange(ref IsResourseUsingNow, 1);
        }
    }
}
