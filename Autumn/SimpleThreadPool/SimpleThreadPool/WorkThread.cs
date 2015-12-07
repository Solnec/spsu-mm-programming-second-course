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
        private int _isNotWorkedInt = 1;
        private bool _isSucc;
        private Action _action;
        private AutoResetEvent _initAction = new AutoResetEvent(false);
        private bool _end;
        private object _lock = new object();

        public int Id
        {
            get;
            private set;
        }


        public WorkThread(int id)
        {
            Id = id;
            _thread = new Thread(Action);
            _thread.Start();
        }

        public bool InitAction(Action action)
        {
            _isSucc = (Interlocked.CompareExchange(ref _isNotWorkedInt, 0, 1) == 1);
            if (_isSucc)
            {
                _action = action;
                _initAction.Set();
            }
            return _isSucc;
        }


        private void Action()
        {
            while (!_end)
            {
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
                _isNotWorkedInt = 1;
            }

        }

        public void Dispose()
        {
            _end = true;
            lock (_lock)
            {
                _action = null;
            }
            _initAction.Set();
            _thread.Join();
        }
    }
}
