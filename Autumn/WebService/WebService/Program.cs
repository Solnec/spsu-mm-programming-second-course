using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Diagnostics;

namespace WebService
{
    class Program
    {
        static bool isRunning = true;
        static int max;

        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(WebService), new Uri("net.tcp://localhost:1111/"));
            host.AddServiceEndpoint(typeof(IWebService), new NetTcpBinding(SecurityMode.None), "");
            host.Open();
            Console.WriteLine("Host started.");

            while (isRunning)
            {
                Thread t = new Thread(Sorting);
                t.Start();
                max++;
            }

            Console.WriteLine("Number of threads: {0}", max);
            Console.ReadLine();
            host.Close();
            Console.WriteLine("Host closed.");
            Console.ReadLine();
        }

        static void Sorting()
        {
            try
            {
                ChannelFactory<IWebService> cf = new ChannelFactory<IWebService>(new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://localhost:1111"));
                IWebService client = cf.CreateChannel();
                int[] array = { 1, -2, 0, 145, 78, 98, 554, 123, -123, 111, 3, 9, 45, 432, 111111, 90, 77, -1000000, 7, 908070 };
                int[] sortedArray = client.BubbleSort(array);
            }
            catch (Exception)
            {
                isRunning = false;
            }
        }
    }
}
