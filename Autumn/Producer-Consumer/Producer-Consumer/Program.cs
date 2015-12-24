// Producer-Consumer problem 
// Leonova Anna
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Producer_Consumer
{
    public static 
    class Program
    {        
        private static int count_produce = 0;
        private static int count_consume = 0;
        private static int n = 5;
        private static volatile bool _shouldStop = false;
        private static Thread[] workers;
        private static List<int> Buffer = new List<int>();
        private static Semaphore mutex = new Semaphore(1, 1);
        private static Semaphore isEmpty = new Semaphore(0, 100);
        private static Semaphore isFull = new Semaphore(100, 100);

        static void Main(string[] args)
        {
            workers = new Thread[2 * n];           
            for (int i = 1; i <= n; i++)
            {
                Thread producer = new Thread(new ParameterizedThreadStart(produce));
                Thread consumer = new Thread(new ParameterizedThreadStart(consume));
                producer.Start(i);
                consumer.Start(i);
                workers[2 * i - 2] = producer;
                workers[2 * i - 1] = consumer;
            }
            Console.ReadKey();           
            RequestStop();
            Console.WriteLine();
            for (int j = 1; j <= n; j++)
            {
                workers[2 * j - 2].Join(); Console.WriteLine(" Thread-producer {0} finished", j);
                workers[2 * j - 1].Join(); Console.WriteLine(" Thread-consumer {0} finished", j);
            }

            Console.WriteLine("\n All threads finished working");
            Console.ReadLine();
        }

        private static void produce(object num)
        {
            while (!_shouldStop)
            {                
                Random rng = new Random(100);
                int product = rng.Next();  
                Console.WriteLine("Thread-producer {0} begins " + "and waits for the semaphore.", num);
                isFull.WaitOne();
                mutex.WaitOne();
                count_produce++;
                Buffer.Add(product);
                if (count_produce % 2 == 0) Thread.Sleep(100);
                Console.WriteLine("Thread-producer {0} releases the semaphore.", num);
                mutex.Release();
                isEmpty.Release();
                Thread.Sleep(400);
            }
        }

        public static void consume(object num)
        {
            while (!_shouldStop)
            {                
                Console.WriteLine("Thread-consumer {0} begins " + "and waits for the semaphore.", num);
                isEmpty.WaitOne();
                mutex.WaitOne();
                Console.WriteLine("Thread-consumer {0} захвачен семафором", num);
                int product = Buffer[0];
                Buffer.Remove(product);
                count_consume++;                         
                if (count_consume % 2 == 0) Thread.Sleep(100);
                Console.WriteLine("Thread-consumer {0} releases the semaphore.", num);
                mutex.Release();
                isFull.Release();
                Thread.Sleep(400);
            }
        }

        public static void RequestStop()
        {
            _shouldStop = true;            
        }
    }
}
