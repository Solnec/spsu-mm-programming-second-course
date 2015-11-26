using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class Chat : IChat
    {
        public void Send(string sender, string message)
        {
            Console.WriteLine("{0}: {1}", sender, message);
        }
    }
}
