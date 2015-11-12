using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class MyMutex
    {
        private int _sl;

        public MyMutex(bool initial = true)
        {
            _sl = initial ? 1 : 0;
        }

        public void Wait()
        {
            int b;
            b = Interlocked.CompareExchange(ref _sl, 0, 1);
            while (b != 1)
            {
                Thread.Sleep(0);
                b = Interlocked.CompareExchange(ref _sl, 0, 1);
            }
        }

        public void Release()
        {
            _sl = 1;
        }
    }
}
