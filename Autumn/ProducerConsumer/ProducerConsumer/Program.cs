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

            Console.WriteLine("Please, enter a number of producers");
            while (!result)
                result = Int32.TryParse(Console.ReadLine(), out Producers);

            Console.WriteLine("Now, please, enter a number of consumers");
            result = false;
            while (!result)
                result = Int32.TryParse(Console.ReadLine(), out Consumers);

            ProducerConsumer task = new ProducerConsumer();
            
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

            task.StartTasks();
            Console.ReadKey();
            task.CompleteTasks();
        }
    }
}
