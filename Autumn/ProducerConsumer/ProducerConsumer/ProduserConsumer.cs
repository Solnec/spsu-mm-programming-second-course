using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class ProducerConsumer
    {
        private  bool _flag = true;
        private AtomicQueue<int> _queue = new AtomicQueue<int>();

        private int _producer;
        private int _consumer;

        public ProducerConsumer(int produser, int consumer)
        {
            _producer = produser;
            _consumer = consumer;
        }

        public void Start()
        {
            List<Thread> producerList = new List<Thread>();
            for (int i = 0; i < _producer; i++)
            {
                producerList.Add(new Thread(Producer));
            }

            List<Thread> consumerList = new List<Thread>();
            for (int i = 0; i < _consumer; i++)
            {
                consumerList.Add(new Thread(Consumer));
            }

            foreach (var thread in producerList)
            {
                thread.Start();
            }
            foreach (var thread in consumerList)
            {
                thread.Start();
            }
        }

        public void Stop()
        {
            _flag = false;
        }

        private void Producer()
        {
            int count = 0;
            while (_flag)
            {
                _queue.Enqueue(count);
                count++;
                Thread.Sleep(0);
            }
        }

        private void Consumer()
        {
            while (_flag)
            {
                _queue.Dequeue();
                Thread.Sleep(0);
            }
        }


    }
}
