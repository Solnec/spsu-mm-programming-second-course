using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Problem
{
    class Program
    {
        const int numProducer = 8;
        const int numConsumer = 5;
        public static List<Elements> Buffer = new List<Elements>();

        static void Main(string[] args)
        {
            Mutex wait = new Mutex();
            Producer[] producers = new Producer[numProducer];
            Consumer[] consumers = new Consumer[numConsumer];
            int max = Math.Max(numProducer, numConsumer);
            Thread[] threads = new Thread[max];

            for (int i = 0; i < max; i++)
            {
                if (i < numProducer)
                {
                    producers[i] = new Producer(wait, Buffer);
                    threads[i] = new Thread(producers[i].PutIn);
                    threads[i].Start();                   
                }
                if (i < numConsumer)
                {
                    consumers[i] = new Consumer(wait, Buffer);
                    threads[i] = new Thread(consumers[i].Get);
                    threads[i].Start();
                }
            }

            for (int i = 0; i < max; i++)              
            {
                threads[i].Join();
            }
             
            Console.WriteLine("\nFinished.");
            Thread.Sleep(3000);
            Console.ReadKey();
        }
    }
}