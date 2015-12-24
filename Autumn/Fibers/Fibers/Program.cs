using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fibers
{
    class Program
    {
        static void Main(string[] args)
        {
            int number;
            do
            {
                Console.WriteLine("Enter number of processes.");
            } while (!Int32.TryParse(Console.ReadLine(), out number));

            for (int i = 0; i < number; i++)
            {
                Process process = new Process();
                ProcessManager.Add(process);
            }
            ProcessManager.Switch(false);
            Console.ReadKey();
        }
    }
}
