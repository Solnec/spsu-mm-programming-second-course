using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    public class AtomicQueue<T>
    {
        private MyMutex _mutex = new MyMutex();
        private Queue<T> _queue = new Queue<T>();

        public T Dequeue()
        {
            _mutex.Wait();
            if (_queue.Count == 0)
            {
                _mutex.Release();
                return default(T);
            }
            T tmp = _queue.Dequeue();
            _mutex.Release();
            return tmp;
        }

        public void Enqueue(T value)
        {
            _mutex.Wait();
            _queue.Enqueue(value);
            _mutex.Release();
        }


    }
}
