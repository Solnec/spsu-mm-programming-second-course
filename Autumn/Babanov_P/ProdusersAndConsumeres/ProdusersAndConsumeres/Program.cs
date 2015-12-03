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
        static int AmountOfProduser = 6;
        static int AmountOfConsumer = 6;
        static List<int> Stack;
        static Mutex Lock = new Mutex();
        static void Main(string[] args)
        {
            Produser[] produsers = new Produser[AmountOfProduser];
            Consumer[] consumers = new Consumer[AmountOfConsumer];
            int amountOfThreads = AmountOfConsumer + AmountOfProduser;
            Thread[] threads = new Thread[amountOfThreads];
            Stack = new List<int>();
            for(int i = 0; i < AmountOfProduser; i++)
            {
                produsers[i] = new Produser(i);
            }
            for(int i = 0; i<AmountOfConsumer; i++)
            {
                consumers[i] = new Consumer(i);
            }
            for(int i =0; i<AmountOfProduser; i++)
            {
                threads[i] = new Thread(produsers[i].Work);
                threads[i].Start();
            }
            for (int i = AmountOfProduser; i < amountOfThreads; i++)
            {
                threads[i] = new Thread(consumers[i - AmountOfProduser].Work);
                threads[i].Start();
            }
            
            for(int i = 0; i<amountOfThreads; i++)
            {
                threads[i].Join();
            }
        }
        class Produser
        {
            Random RandomMachine;
            public Produser(int n)
            {
                RandomMachine = new Random(n);
            }
            public void Work()
            {
                while(true)
                {
                    if (Console.KeyAvailable)
                    {
                        break;
                    }
                    
                    int tmp = RandomMachine.Next(500);
                    Lock.WaitOne();
                    Stack.Add(tmp);
                    Lock.ReleaseMutex();
                    Console.WriteLine("I get to list number {0}", tmp);
                    int timeSleep = 500 + RandomMachine.Next(1500);
                    
                    Thread.Sleep(timeSleep);
                }
            }           
        }
        class Consumer
        {
            Random RandomMachine;

            public Consumer(int n)
            {
                RandomMachine = new Random(n);
            }
            public void Work()
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        break;
                    }
                    int timeSleep = 500 + RandomMachine.Next(1500);
                    Lock.WaitOne();
                    if (Stack.Count == 0)
                    {
                        Lock.ReleaseMutex();
                        Thread.Sleep(timeSleep);
                    }
                    else
                    {
                        int tmp = Stack.Last();
                        Stack.Remove(Stack.Last());
                        Lock.ReleaseMutex();
                        Console.WriteLine("I set into list number {0}", tmp);
                        Thread.Sleep(timeSleep);
                    }
                    
                }
            }
        }
    }
}
