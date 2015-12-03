using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using MyWCFService;

namespace MyWCFService
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IService> cf = new ChannelFactory<IService>(new NetTcpBinding(), "net.tcp://127.0.0.1:11000/");
            List<IService> channels = new List<IService>();
            bool err = false;
            int k = 1;

            Console.WriteLine("Введите число элементов массива");
            int Lenght = Convert.ToInt32(Console.ReadLine());
            int[] A = new int[Lenght];

            Random rnd = new Random();
            for (int i = 0; i < Lenght; i++)
            {
                A[i] = rnd.Next(-50, 50);
                Console.Write("{0}, ", A[i]);
            }

            while (!err)
            {
                try
                {
                    channels.Add(cf.CreateChannel());
                    A = channels[channels.Count - 1].Sort(A);
                    Console.Write("Ответ есть!\n");
                    k++;
                }
                catch (TimeoutException)
                {
                    err = true;
                }
            }

            Console.WriteLine("\nСервис выдерживает не более {0} клиентов", k - 1);
            Console.ReadKey();
        }
    }
}
