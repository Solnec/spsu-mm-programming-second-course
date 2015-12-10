using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerComsumer
{
    class ProducersAndConsumers
    {
        public void Add(bool running, List<int> lst, Thread thread)
        {
            int a;

            lock (lst)
            {
                if (!running)
                {
                    Monitor.Pulse(lst);
                    return;
                }

                a = new Random().Next(0, 1000);
                Console.WriteLine(a.ToString() + " added by thread #" + thread.ManagedThreadId);
                lst.Add(a);
                Console.Write("New list is: ");
                for (int i = 0; i < lst.Count; i++) Console.Write("{0} ", lst[i]);
                Console.WriteLine();
                Console.WriteLine();

                Thread.Sleep(5);

                Monitor.Pulse(lst);
                Monitor.Wait(lst);
            }
        }

        public void Delete(bool running, List<int> lst, Thread thread)
        {
            lock (lst)
            {
                if (!running)
                {
                    Monitor.Pulse(lst);
                    return;
                }

                if (lst.Count > 0)
                {
                    int a = new Random().Next(0, lst.Count - 1);
                    lst.Remove(lst[a]);
                    Console.WriteLine(lst[a].ToString() + " removed by thread #" + thread.ManagedThreadId);
                    Console.Write("New list is: ");
                    for (int i = 0; i < lst.Count; i++) Console.Write("{0} ", lst[i]);
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else Console.WriteLine("List is empty!");

                Thread.Sleep(5);

                Monitor.Pulse(lst);
                Monitor.Wait(lst);
            }
        }
    }
}
