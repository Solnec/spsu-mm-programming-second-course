using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleThreadPool
{
    public class SimpleThreadPool: IDisposable
    {
        private Queue<Action> _taskQueue = new Queue<Action>();
        private List<WorkThread> _workThreadList = new List<WorkThread>();
        private object _in = new object();
        private object _out = new object();
        private Thread _primeThread;
        private bool _isEnd;
        public SimpleThreadPool(int number)
        {
            for (int i = 0; i < number; i++)
            {
                _workThreadList.Add(new WorkThread(i));
            }
            _primeThread = new Thread(Start);
            _primeThread.Start();
        }

        private void Start()
        {
            while (!_isEnd)
            {
                InitAction();
            }
        }

        private void InitAction()
        {
            lock (_out)
            {
                if (_taskQueue.Count == 0)
                    return;
                Action tmp = _taskQueue.Dequeue();
                while (true)
                {
                    foreach (var workThread in _workThreadList)
                    {
                        if (workThread.InitAction(tmp))
                            return;
                    }
                    Thread.Sleep(0);
                }

            }
        }

        public void Enqueue(Action action)
        {
            lock (_in)
            {
                _taskQueue.Enqueue(action);
            }
        }

        public void Dispose()
        {
            _isEnd = true;
            _primeThread.Join();
            foreach (var workThread in _workThreadList)
            {
                workThread.Dispose();
            }
            _workThreadList.Clear();
            _taskQueue.Clear();
        }
    }
}
