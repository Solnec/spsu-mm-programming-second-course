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
        public static bool stoptime = true;
        public static List<int> ValuesList = new List<int>();

        static void Main(string[] args)
        {       
            var pr = new Producer(ValuesList);
            var con = new Consumer(ValuesList);
            Thread producerthread = new Thread(pr.Producermethod);
            producerthread.Start();
            Thread consumerthread = new Thread(con.Consumermethod);
            consumerthread.Start();
            Console.ReadKey();
            stoptime = false;
            producerthread.Join();
            consumerthread.Join();
            Console.WriteLine("A stop button was pushed");
        }
    }
}
