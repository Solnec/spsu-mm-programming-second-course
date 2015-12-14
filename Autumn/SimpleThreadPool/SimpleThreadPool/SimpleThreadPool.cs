using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleThreadPool
{
    public class SimpleThreadPool : IDisposable
    {
        public int Count
        {
            get;
            private set;
        }
        private Queue<Action> _taskQueue = new Queue<Action>();
        private List<WorkThread> _workThreadList = new List<WorkThread>();
        private object _lock = new object();
        private SemaphoreSlim _countQueue = new SemaphoreSlim(0);
        public SimpleThreadPool(int number)
        {
            Count = number;
            for (int i = 0; i < number; i++)
            {
                _workThreadList.Add(new WorkThread(i, InitAction, _countQueue));
            }
        }



        private void InitAction(int id)
        {
            _countQueue.Wait();
            lock (_lock)
            {
                if (_taskQueue.Count == 0)
                    return;
                _workThreadList[id].InitAction(_taskQueue.Dequeue());
            }
        }

        public void Enqueue(Action action)
        {
            lock (_lock)
            {
                _taskQueue.Enqueue(action);
                _countQueue.Release();
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _taskQueue.Clear();
                _countQueue.Release(Count);
            }
            foreach (var workThread in _workThreadList)
            {
                workThread.Dispose();
            }
            _workThreadList.Clear();
        }
    }
}
