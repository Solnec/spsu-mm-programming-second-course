using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Routing;
using System.ServiceModel.Description;

namespace TestApplication
{
    class Program
    {
        static bool Stop = false;
        static Mutex Lock = new Mutex();
        static void Main(string[] args)
        {
            List<Thread> Threads = new List<Thread>();
            while(true)
            {
                Lock.WaitOne();
                if(Stop)
                {
                    Lock.ReleaseMutex();
                    break;
                }
                Lock.ReleaseMutex();
                Threads.Add(new Thread(Tester));
                Threads.Last<Thread>().Start();
                Thread.Sleep(0);
                Console.WriteLine("started {0} threads", Threads.Count);
            }
            int Amount = Threads.Count;
            foreach(Thread n in Threads)
            {
                n.Join();
            }
            Console.WriteLine(Amount);
            Console.ReadKey();
        }
        static void Tester()
        {
            Random randomMachine = new Random();
            int lines, columns, ovrerallNumbers;
            lines = randomMachine.Next(30);
            columns = randomMachine.Next(30);
            ovrerallNumbers = randomMachine.Next(30);
            int[,] m1 = new int[lines, ovrerallNumbers];
            int[,] m2 = new int[ovrerallNumbers, columns];
            for(int i = 0; i<lines; i++)
            {
                for (int j = 0; j < ovrerallNumbers; j++)
                    m1[i, j] = randomMachine.Next(100);
            }
            for(int i = 0; i<ovrerallNumbers;i++)
            {
                for (int j = 0; j < columns; j++)
                    m2[i, j] = randomMachine.Next(100);
            }
            while (true)
            {
                Lock.WaitOne();
                if (Stop)
                {
                    Lock.ReleaseMutex();
                    break;
                }
                Lock.ReleaseMutex();
                try
                {
                    using (ChannelFactory<WCFService.IService> Factory = new ChannelFactory<WCFService.IService>(new WebHttpBinding(), "localhost:8000"))
                    {
                        Factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
                        WCFService.IService Channel = Factory.CreateChannel();
                        Channel.MatrixMultiplication(m1, m2);
                    }
                }
                catch(Exception)
                {
                    Lock.WaitOne();
                    Stop = true;
                    Lock.ReleaseMutex();
                    break;
                }
                Thread.Sleep(0);
            }
        }
    }
}
