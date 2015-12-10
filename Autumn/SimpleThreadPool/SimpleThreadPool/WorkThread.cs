using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleThreadPool
{
    public class WorkThread : IDisposable
    {
        private Thread _thread;
        private Action _action;
        private AutoResetEvent _initAction = new AutoResetEvent(false);
        private bool _end;
        private object _lock = new object();
        private SemaphoreSlim _countQueue;

        public delegate void InitWork(int id);

        private event InitWork _nextAction;


        public int Id
        {
            get;
            private set;
        }


        public WorkThread(int id, InitWork init, SemaphoreSlim sem)
        {
            Id = id;
            _nextAction += init;
            _countQueue = sem;
            _thread = new Thread(Action);
            _thread.Start();
        }

        public void InitAction(Action action)
        {
                _action = action;
                _initAction.Set();
        }


        private void Action()
        {
            while (!_end)
            {
                lock(_lock)
                    if (_nextAction != null) _nextAction(Id);
                _initAction.WaitOne();
                lock (_lock)
                {
                    if (_action != null)
                    {
                        Console.WriteLine("Start {0}", Id);
                        _action();
                        Console.WriteLine("Finish {0}", Id);
                    }
                }
            }

        }

        public void Dispose()
        {
            _end = true;
            lock (_lock)
            {
                _nextAction = null;
                _action = null;
            }
            _initAction.Set();
            _thread.Join();
        }
    }
}
