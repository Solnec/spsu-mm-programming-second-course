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
        public static Semaphore empty = new Semaphore(3,3);
        public static Semaphore full = new Semaphore(0,2);
        public static Semaphore access = new Semaphore(1, 3);
    }
}
