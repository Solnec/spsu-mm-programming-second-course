using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace ProducerConsumer
{
    class Producer
    {
        public static List<int> ValuesList = new List<int>();
        public static int producernumber = 0;
        public static byte check = 0;
        ConsoleKeyInfo stop = new ConsoleKeyInfo();
        public Producer(List<int> valueslist)
        {
            ValuesList = valueslist;
        }
       
        public void Producermethod()
        {
            Random rnd = new Random();
            while (Program.stoptime)
            {
                Semaphores.empty.WaitOne();               
                Semaphores.access.WaitOne();
                int a = rnd.Next(100);
                producernumber++; 
                ValuesList.Add(a);
                Console.WriteLine("Producer #{1} adds {0} to the list", a, producernumber);
                check++;
                Semaphores.access.Release();               
                try
                { 
                    Semaphores.full.Release();
                }
                catch (SemaphoreFullException) { }              
            }
        }

    }
}
