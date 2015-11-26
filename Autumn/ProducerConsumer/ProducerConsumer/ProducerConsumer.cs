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
        int IsProcess = 1;
        List<int> Buffer = new List<int>();
        MySynch reset = new MySynch();

        public void TryPutIn(int Index)
        {
            while (IsProcess == 1)
            {
                reset.TryEnter();
                PutIn(Index);
                reset.Set();
                Thread.Sleep(1000);
            }
            Console.WriteLine("Producer[{0}] finished his work", Index);
        }

        public void TryTakeFrom(int Index)
        {
            while (IsProcess == 1)
            {
                while (Buffer.Count == 0)
                    Thread.Sleep(1);

                reset.TryEnter();
                TakeFrom(Index);
                reset.Set();
                Thread.Sleep(1000);
            }
            Console.WriteLine("Consumer[{0}] finished his work", Index);
        }

        public void CompleteTasks()
        {
            IsProcess = 0;
            reset.Set();
            Thread.Sleep(2000);

            Console.WriteLine("\nSuccess!");
            Console.WriteLine("List.Count = {0}", Buffer.Count);
            Console.WriteLine("Last Element = {0}", Buffer[Buffer.Count - 1]);
            Console.ReadKey();
        }

        public void StartTasks()
        {
            reset.Set();
        }

        private void PutIn(int Index)
        {
            Random rnd = new Random();
            int item = Index + rnd.Next(-50, 50);
            Buffer.Add(item);
            Console.WriteLine("Producer[{0}] add {1}", Index, item);
        }

        private void TakeFrom(int Index)
        {
            int reader = Buffer[Buffer.Count - 1];

            Console.WriteLine("Consumer[{0}] = {1}, List[LastIndex] = {2}", Index, reader, Buffer[Buffer.Count - 1]);
        }
    }
}
