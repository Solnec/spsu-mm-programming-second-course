using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {

        const int pValue = 2;
        const int cValue = 3;
        static List<Thread> producers;
        static List<Thread> consumers;
        static int check = 1;
        static int pNumber = 0;
        static int cNumber = 0;
        static bool process = true;
        static int stop = 1;

        static List<int> objects;

        static Random rnd = new Random();
        static void Main(string[] args)
        {
            objects = new List<int>();
            producers = new List<Thread>();
            consumers = new List<Thread>();

            Console.WriteLine("To finish press any Key");

            for (int i = 0; i < pValue; ++i)
            {
                var thread = new Thread(Producer);
                thread.Name = "Producer" + i.ToString();
                thread.Start(i);
                producers.Add(thread);
            }

            for (int i = 0; i < cValue; ++i)
            {
                var thread = new Thread(Consumer);
                thread.Name = "Consumer" + i.ToString();
                thread.Start(i);
                consumers.Add(thread);

            }

            Thread keyCheck = new Thread(ThreadProc);
            keyCheck.Start();

        }

        //static void Push(int data)
        //{
        //    int current = Interlocked.CompareExchange(ref check, 0, 1);
        //    while (current == 0)
        //    {
        //        Thread.Sleep(0);
        //        current = Interlocked.CompareExchange(ref check, 0, 1);
        //    }
        //    objects.Add(data);

        //}

        //static int? Pop()
        //{
        //    int initialValue;
        //    int current = Interlocked.CompareExchange(ref check, 0, 1);
        //    while (current == 0)
        //    {
        //        Thread.Sleep(0);
        //        current = Interlocked.CompareExchange(ref check, 0, 1);
        //    }
        //    if (objects.Count == 0)
        //    {
        //        return null;
        //    }
        //    initialValue = objects.Last();
        //    return initialValue;
        //}

        static void Producer(object number)
        {
            for (; ; )
            {
                //Push((int)data);
                int current = Interlocked.CompareExchange(ref check, 0, 1);
                while (current == 0)
                {
                    Thread.Sleep(0);
                    current = Interlocked.CompareExchange(ref check, 0, 1);
                }
                int stopCurrent = Interlocked.CompareExchange(ref stop, 0, 1);
                while (stopCurrent == 0)
                {
                    Thread.Sleep(0);
                    stopCurrent = Interlocked.CompareExchange(ref stop, 0, 1);
                }

                if (!process)
                {
                    Console.WriteLine("Producer {0} has finished his work", (int)number + 1);
                    stop = 1;
                    check = 1;
                    return;
                }
                stop = 1;
                int data = rnd.Next(1000);
                objects.Add(data);
                Console.WriteLine("Producer {0} has added {1} to list", (int)number + 1, data);
                check = 1;
                Thread.Sleep(0);
            }
            return;
        }

        static void Consumer(object number)
        {
            for (; ; )
            {
                int data;
                int current = Interlocked.CompareExchange(ref check, 0, 1);
                while (current == 0)
                {
                    Thread.Sleep(0);
                    current = Interlocked.CompareExchange(ref check, 0, 1);
                }
                int stopCurrent = Interlocked.CompareExchange(ref stop, 0, 1);
                while (stopCurrent == 0)
                {
                    Thread.Sleep(0);
                    stopCurrent = Interlocked.CompareExchange(ref stop, 0, 1);
                }

                if (!process)
                {
                    Console.WriteLine("Consumer {0} has finished his work", (int)number + 1);
                    stop = 1;
                    check = 1;
                    return;
                }

                if(objects.Count == 0)
                {
                    check = 1;
                    continue;
                }

                stop = 1;
                data = objects.Last();
                //int? data = Pop();
                Console.WriteLine("Consumer {0} has got {1}", (int)number + 1, data);
                check = 1;
                Thread.Sleep(0);
            }
            return;
        }

        static void ThreadProc()
        {
            while (process)                //??
            {
                //stop = 1;
                //Thread.Sleep(0);
                //foreach (var p in producers)
                //{
                //    if(p.ThreadState == ThreadState.Stopped)
                //    {
                //        p.Start(producers.IndexOf(p));
                //    }
                //}

                //foreach (var c in consumers)
                //{
                //    if (c.ThreadState == ThreadState.Stopped)
                //    {
                //        c.Start(producers.IndexOf(c));
                //    }
                //}

                //int current = Interlocked.CompareExchange(ref stop, 0, 1);
                //while (current == 0)
                //{
                //    Thread.Sleep(0);
                //    current = Interlocked.CompareExchange(ref stop, 0, 1);
                //}
                if (Console.ReadKey() != null)
                {
                    int current = Interlocked.CompareExchange(ref stop, 0, 1);
                    while (current == 0)
                    {
                        Thread.Sleep(0);
                        current = Interlocked.CompareExchange(ref stop, 0, 1);
                    }
                    process = false;
                    stop = 1;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            Console.ReadKey();
        }
    }
}

