using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WCF_Chat_2._0
{
    class Interface
    {
        public event SendUsersMessage EnteredNewMessage;
        public event EndWork ItsTimeToEnd;
        public string Name;
        public void WriteToConsole (string s)
        {
            Console.WriteLine(s);
        }

        public void Start()
        {
            while(true)
            {
                string s = Console.ReadLine();
                if(s == "-q")
                {
                    break;                
                }
                EnteredNewMessage(Name + ": " + s);
                Thread.Sleep(0);
                
            }
        }
    }
}
