using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Consumer
    {
        public static int consumernumber = 0;
        public static byte check1 = 0;
        ConsoleKeyInfo stop = new ConsoleKeyInfo();
        public Consumer(List<int> valueslist)
        {
            Producer.ValuesList = valueslist;
        }
        public void Consumermethod()
        {
            while (Program.stoptime)
            {
                Semaphores.full.WaitOne();
                Semaphores.access.WaitOne();
                Console.WriteLine("Consumer #{1} takes {0} from the list", Producer.ValuesList[Producer.producernumber - 1], Producer.producernumber - 1);
                check1++;
                   consumernumber++;
                   Producer.producernumber = Producer.producernumber - consumernumber;
                   Producer.ValuesList.RemoveAt(Producer.producernumber);
                if (check1 >= 2 & Producer.check >= 2)
                {
                    Thread.Sleep(3000);
                    check1 = 0;
                    Producer.check = 0;
                }
                   consumernumber--;
                   Semaphores.access.Release();
                try
                {
                    Semaphores.empty.Release();
                }
                catch (SemaphoreFullException) { }
            }
        }
    }
}
