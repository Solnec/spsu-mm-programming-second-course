using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Problem
{
    class Producer
    {
        public List<Elements> buffer = new List<Elements>();
        Mutex wait  = new Mutex();

        public Producer(Mutex wait, List<Elements> buffer)
        {
            this.wait = wait;
            this.buffer = buffer;
        }

        public void PutIn()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                    break;
                wait.WaitOne();
                //Случайные числа
                Random x = new Random();                   
                int n = x.Next(10);// возвращает целое положительное число, не больше указанного
                buffer.Add(new Elements((int)(n)));
                Console.WriteLine("Put in buffer " + n);
                wait.ReleaseMutex();
            }
        }
    }
}
