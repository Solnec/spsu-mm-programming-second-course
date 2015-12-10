using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerComsumer
{
    class MyThread
    {
        public Thread thrd;
        public bool State;
        ProducersAndConsumers TickTackObject;
        static public List<int> ListOfInts = new List<int> { 0, 12, 3 };

        public MyThread(string name, ProducersAndConsumers ttobj, bool status)
        {
            this.State = status;
            thrd = new Thread(this.Run);
            thrd.Name = name;
            this.TickTackObject = ttobj;
            thrd.Start();
        }

        void Run()
        {
            if (thrd.Name == "Producer")
            {
                while (State) TickTackObject.Add(true, ListOfInts, thrd);
                TickTackObject.Add(false, ListOfInts, thrd);
            }
            else
            {
                while (State) TickTackObject.Delete(true, ListOfInts, thrd);
                TickTackObject.Delete(false, ListOfInts, thrd);
            }
        }
    }
}
