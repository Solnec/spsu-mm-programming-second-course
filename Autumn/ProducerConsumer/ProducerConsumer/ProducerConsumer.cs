using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class ProducerConsumer
    {
        public int IsProcess = 1;
        List<int> Buffer = new List<int>();
        int IsAllRead = 1;

        public ProducerConsumer(int CountConsumers, int CountProducers)
        {
            WasRead = new int[CountConsumers];
            Added = new int[CountProducers];
        }

        int IsAnyProducingNow = 0;
        int CountProducers = 0;
        int[] Added;

        public void TryPutIn(int Index)
        {
            while (Interlocked.Equals(IsProcess, 1))
            {
                if ((Interlocked.Equals(IsAllRead, 1)) && (Added[Index] == 0))
                {
                    if (0 == Interlocked.CompareExchange(ref IsAnyProducingNow, 1, 0))
                    {
                        PutIn(Index);
                        Interlocked.Exchange(ref IsAllRead, 0);
                        Thread.Sleep(200);
                        Interlocked.Exchange(ref IsAnyProducingNow, 0);
                    }
                }
            }
            Console.WriteLine("Producer[{0}] finished his work", Index);
        }

        int IsAnyReadersNow = 0;
        int CountConsumers = 0;
        int[] WasRead;

        public void TryTakeFrom(int Index)
        {
            while (Interlocked.Equals(IsProcess, 1))
            {
                if ((Interlocked.Equals(IsAllRead, 0)) && (WasRead[Index] == 0))
                {
                    if (0 == Interlocked.CompareExchange(ref IsAnyReadersNow, 1, 0))
                    {
                        TakeFrom(Index);
                        Thread.Sleep(200);
                        Interlocked.Exchange(ref IsAnyReadersNow, 0);
                    }
                }
            }
            Console.WriteLine("Consumer[{0}] finished his work", Index);
        }

        public void CompleteTasks()
        {
            Interlocked.Exchange(ref IsProcess, 0);
            Thread.Sleep(2000);

            Console.WriteLine("\nSuccess!");
            Console.WriteLine("List.Count = {0}", Buffer.Count);
            Console.WriteLine("Last Element = {0}", Buffer[Buffer.Count - 1]);
            Console.ReadKey();
        }

        private void PutIn(int Index)
        {
            Random rnd = new Random();
            int item = Index + rnd.Next(-50, 50);
            Buffer.Add(item);
            Added[Index] = 1;
            CountProducers++;

            Console.WriteLine("Producer[{0}] add {1}", Index, item);

            if (CountProducers == Added.Length)
            {
                for (int i = 0; i < Added.Length; i++)
                    Interlocked.Exchange(ref Added[i], 0);

                CountProducers = 0;
                Console.WriteLine("All producers have added!!!");
            }
        }

        private void TakeFrom(int Index)
        {
            int reader = Buffer[Buffer.Count - 1];
            WasRead[Index] = 1;
            CountConsumers++;

            Console.WriteLine("Consumer[{0}] = {1}, List[LastIndex] = {2}", Index, reader, Buffer[Buffer.Count - 1]);

            if (CountConsumers == WasRead.Length)
            {
                for (int i = 0; i < WasRead.Length; i++)
                    Interlocked.Exchange(ref WasRead[i], 0);

                CountConsumers = 0;
                Console.WriteLine("All consumers have readed!!!");
                Interlocked.Exchange(ref IsAllRead, 1);
            }
        }
    }
}
