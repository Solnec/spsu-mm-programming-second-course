using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ProducerConsumer myConsumer = new ProducerConsumer(20, 20);
            myConsumer.Start();
            Console.ReadKey();
            myConsumer.Stop();
        }
    }
}
