using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerComsumer
{
    class Program
    {
        static void Main()
        {
            ProducersAndConsumers PaCObj = new ProducersAndConsumers();
            int NumberOfProducers = 5;
            int NumberOfConsumers = 5;
            MyThread[] threads = new MyThread[NumberOfConsumers + NumberOfProducers];
            for (int i = 0; i < NumberOfConsumers + NumberOfProducers; i++)
            {
                if (i < NumberOfProducers) threads[i] = new MyThread("Producer", PaCObj, true);
                else threads[i] = new MyThread("Consumer", PaCObj, true);
            }

            Console.ReadKey(true);

            foreach (MyThread thread in threads)
            {
                thread.State = false;
            }

            Console.ReadLine();
        }
    }
}
