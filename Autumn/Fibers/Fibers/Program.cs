using System;
using System.Diagnostics;
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

            for (int i = 0; i < 4; i++)
            {
                ProcessManager.Process process = new ProcessManager.Process();
                ProcessManager.ProcessManager.ListOfProcesses.Add(process);
                Fiber fiber = new Fiber(new Action(process.Run));
                ProcessManager.ProcessManager.ListOfFibers.Add(fiber.Id);
            }
            ProcessManager.ProcessManager.Switch(false);
            Console.ReadLine();
        }
    }
}
