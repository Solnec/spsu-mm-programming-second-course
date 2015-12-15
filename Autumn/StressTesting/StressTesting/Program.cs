using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using System.Threading;

namespace StressTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(SortService), new Uri("net.tcp://localhost:8000/"));

            host.AddServiceEndpoint(typeof(ISortService),
                                    new NetTcpBinding(SecurityMode.None)
                                    , "service");

            host.Open();

            for (int i = 0; i < 300; i++)
            {
                Thread t = new Thread(Test);
                t.Start();
            }

            Console.ReadKey();
            host.Close();
        }

        static void Test()
        {
            var cf = new ChannelFactory<ISortService>(new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://localhost:8000/service"));
            var client = cf.CreateChannel();
            int[] array = { 9, 6, 8, 7, 3, 4, 1, 0, 5, 2, 65, 64, 12, 63, 95, 47, 19 };
            int[] sortedArray = client.Sort(array);
            Console.WriteLine("Succ\n" + Thread.CurrentThread.ManagedThreadId);
        }

    }
}
