using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Problem
{
    class Consumer
    {
        public List<Elements> buffer = new List<Elements>();
        Mutex wait = new Mutex();

        public Consumer(Mutex wait, List<Elements> buffer)
        {
            this.wait = wait;
            this.buffer = buffer;
        }

        public void Get()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                    break;
                wait.WaitOne();
                if (buffer.Count == 0)
                {
                    Console.WriteLine("Empty");
                    wait.ReleaseMutex();
                }
                else
                {
                    Console.WriteLine("Consumed: {0}", buffer[buffer.Count - 1].Value);
                    buffer.RemoveAt(buffer.Count - 1);
                    wait.ReleaseMutex();
                }
            }
        }
    }
}
           

