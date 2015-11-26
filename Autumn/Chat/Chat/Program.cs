using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the p2p chat.");
            Console.WriteLine("Enter your name.");
            string name = Console.ReadLine();
            var user = new User(name);
            var userThread = new Thread(user.Run) { IsBackground = true };
            userThread.Start();

            while(!user.IsStarted)
                Thread.Sleep(0);

            Console.Write("Enter Something: \n");
            while (true)
            {
                string tmp = Console.ReadLine();

                if (tmp == "/exit") break;

                user.Channel.Send(user.Name, tmp);
            }

            user.Stop();
            userThread.Join();
        }
    }
}
