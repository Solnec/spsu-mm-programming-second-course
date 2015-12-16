using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using MPI;

namespace MPIService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                if (Communicator.world.Rank == 0)
                {
                    ServiceHost host = new ServiceHost(typeof(Service), new Uri("net.tcp://localhost:1234/"));
                    host.AddDefaultEndpoints();

                    host.Open();
                    Console.WriteLine("Сервис открыт для действий!");

                    while (Service.IsAlive == 1)
                    {
                        for (int i = 1; i < Communicator.world.Size; i++)
                            Communicator.world.Send<int>(0, i, 3);
                        Thread.Sleep(100);
                    }

                    for (int i = 1; i < Communicator.world.Size; i++)
                        Communicator.world.Send<int>(-1, i, 3);

                    Thread.Sleep(100);
                    host.Close();
                    Console.WriteLine("\nСервис закрыт!");
                }
                else
                {
                    int message = Communicator.world.Receive<int>(0, 3);
                    Service serv = new Service();
                    int[] A = new int[] { 0, 0, 0, 0, 0 };
                    while (Service.IsAlive == 1)
                    {
                        message = Communicator.world.Receive<int>(0, 3);
                        A = serv.Sort(A);
                    }
                }
            }
        }
    }
}
