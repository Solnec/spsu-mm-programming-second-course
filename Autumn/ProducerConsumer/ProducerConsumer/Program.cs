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
        static void Main(string[] args)
        {
            bool result = false;
            int Producers = 0;
            int Consumers = 0;

            while (!result)
                result = Int32.TryParse(Console.ReadLine(), out Producers);

            result = false;
            while (!result)
                result = Int32.TryParse(Console.ReadLine(), out Consumers);

            ProducerConsumer task = new ProducerConsumer(Consumers, Producers);
            
            int i = 0;
            do
            {
                int Index = i;
                Thread threadProd = new Thread(delegate() { task.TryPutIn(Index); });
                threadProd.Start();
                i++;
            } while (i < Producers);

            i = 0;
            do
            {
                int Index = i;
                Thread threadConsum = new Thread(delegate() { task.TryTakeFrom(Index); });
                threadConsum.Start();
                i++;
            } while (i < Consumers);

            Console.ReadKey();
            task.CompleteTasks();
        }
    }
}
