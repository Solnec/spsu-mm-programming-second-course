using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Semaphores
    {
        public static Semaphore full = new Semaphore(0,3);
        public static Semaphore access = new Semaphore(1, 3);
    }
}
