using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProdusersAndConsumeres
{
    class Program
    {
        static int AmountOfProduser = 5;
        static int AmountOfConsumer = 6;
        static List<int> Stack;
        static Mutex Lock = new Mutex();
        static bool Stop = false;
        static void Main(string[] args)
        {
            Produser[] Produsers = new Produser[AmountOfProduser];
            Consumer[] Consumers = new Consumer[AmountOfConsumer];
            int AmountOfThreads = AmountOfConsumer + AmountOfProduser;
            Thread[] Threads = new Thread[AmountOfThreads];
            Stack = new List<int>();
            for(int i = 0; i < AmountOfProduser; i++)
            {
                Produsers[i] = new Produser();
            }
            for(int i = 0; i<AmountOfConsumer; i++)
            {
                Consumers[i] = new Consumer();
            }
            for(int i =0; i<AmountOfProduser; i++)
            {
                Threads[i] = new Thread(Produsers[i].Work);
                Threads[i].Start();
            }
            for (int i = AmountOfProduser; i < AmountOfThreads; i++)
            {
                Threads[i] = new Thread(Consumers[i - AmountOfProduser].Work);
                Threads[i].Start();
            }
            Console.ReadLine();
            Stop = true;
            for(int i = 0; i<AmountOfThreads; i++)
            {
                Threads[i].Join();
            }
        }
        class Produser
        {
            public void Work()
            {
                Random RandomMachine = new Random();
                while(true)
                {
                    if (Stop)
                    {
                        break;
                    }
                    
                    int tmp = RandomMachine.Next();
                    Lock.WaitOne();
                    Stack.Add(tmp);
                    Lock.ReleaseMutex();
                    Console.WriteLine("I get to list number {0}", tmp);
                    int TimeSleep = 1000 + RandomMachine.Next(3000);
                    
                    Thread.Sleep(TimeSleep);
                }
            }
            ~Produser()
            {
                Lock.Dispose();
            }
            
        }
        class Consumer
        {
            public void Work()
            {
                Random RandomMachine = new Random();
                while (true)
                {
                    if (Stop)
                    {
                        break;
                    }
                    Lock.WaitOne();
                    int tmp = Stack.Last();
                    Stack.Remove(Stack.Count - 1);
                    Console.WriteLine("I set into list number {0}", tmp);
                    int TimeSleep = 1000 + RandomMachine.Next(3000);
                    Lock.ReleaseMutex();
                    Thread.Sleep(TimeSleep);
                }
            }
            ~Consumer()
            {
                Lock.Dispose();
            }
        }
    }
}
