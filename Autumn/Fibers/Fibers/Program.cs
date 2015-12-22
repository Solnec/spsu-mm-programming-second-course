using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 0;
            bool result = false;

            while (!result)
            {
                Console.WriteLine("Введите кол-во процессов");
                result = int.TryParse(Console.ReadLine(), out N);
            }

            ProcessManager.CurrentProccess = 0;
            for (int i = 0; i < N; i++)
            {
                Process process = new Process();
                Console.WriteLine("Priority[{0}] = {1}", i, process.Priority);
                Fiber fiber = new Fiber(delegate() { process.Run(); });
                ProcessManager.Processes.Add(process);
                ProcessManager.Fibers.Add(fiber.Id);
                ProcessManager.FibersForDelete.Add(fiber.Id);
            }

            ProcessManager.Switch(false);
            Console.ReadKey();
        }
    }
}
