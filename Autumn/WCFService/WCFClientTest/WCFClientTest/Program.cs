using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            bool err = false;

            Console.WriteLine("Введите число элементов массива");
            int Length = Convert.ToInt32(Console.ReadLine());
            int[] A = new int[Length];
            int k = 0;

            Random rnd = new Random();
            for (int i = 0; i < Length; i++)
            {
                A[i] = rnd.Next(-50, 50);
                Console.Write("{0}, ", A[i]);
            }

            while (!err)
            {
                Thread t = new Thread(delegate()
                    {
                        try
                        {
                            IService channel = cf.CreateChannel();
                            A = channel.Sort(A);
                        }
                        catch (Exception)
                        {
                            err = true;
                        }
                    }
                    );
                t.IsBackground = true;
                t.Start();
                k++;
            }

            Console.WriteLine("\nСервис выдерживает не более {0} клиентов", k - 1);
            Console.ReadKey();
        }
    }
}
