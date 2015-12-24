using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Fibers;

namespace Fibers
{
    public static class ProcessManager
    {
        private static List<Process> _processesList = new List<Process>();
        private static List<uint> _fibersIdList = new List<uint>();
        private static List<uint> _fibersForDeleteIdList = new List<uint>();
        private static int _currentProccessIndex;
        private static uint _currentFiberId;

        static ProcessManager()
        {
            _currentProccessIndex = 0;
        }

        public static void Add(Process process)
        {
            Fiber fiber = new Fiber(process.Run);
            _processesList.Add(process);
            _fibersIdList.Add(fiber.Id);
            _fibersForDeleteIdList.Add(fiber.Id);
        }

        private static void Next()
        {
            uint f = _fibersIdList[0];
            Process proc = _processesList[0];
            _fibersIdList.Remove(f);
            _processesList.Remove(proc);
            _fibersIdList.Add(f);
            _processesList.Add(proc);
        }

        private static void Priority(bool fiberFinished)
        {
            int maxPriority = Int32.MinValue;
            for (int i = 0; i < _processesList.Count; i++)
            {
                if ((maxPriority < _processesList[i].Priority) && ((_processesList[i] != _processesList[_currentProccessIndex])|| fiberFinished))
                {
                    maxPriority = _processesList[i].Priority;
                    _currentProccessIndex = i;
                }
            }

        }

        //Unpriority dispatch
        /*public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                uint fiberForDelete = _currentFiberId;
                _fibersIdList.Remove(fiberForDelete);
                _processesList.Remove(_processesList[_currentProccessIndex]);
                _currentProccessIndex = 0;
            }
            else
            {
                Next();
            }

            if (_fibersIdList.Count > 0)
            {
                _currentFiberId = _fibersIdList[0];
                Console.WriteLine("Switch on {0} fiber", _currentFiberId);
            }
            else
            {
                foreach (uint f in _fibersForDeleteIdList)
                {
                    if ((f != Fiber.PrimaryId) && (f != _currentFiberId))
                    {
                        Console.WriteLine("Delete fiber{0}", f);
                        Fiber.Delete(f);
                    }
                }
                _currentFiberId = Fiber.PrimaryId;
                Console.WriteLine("END");
            }
            Thread.Sleep(100);
            Fiber.Switch(_currentFiberId);
        }*/

        //Priority dispatch
        public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                uint fiberForDelete = _currentFiberId;
                _fibersIdList.Remove(fiberForDelete);
                _processesList.Remove(_processesList[_currentProccessIndex]);
                _currentProccessIndex = 0;
            }

            if (_fibersIdList.Count > 0)
            {
                Priority(fiberFinished);
                _currentFiberId = _fibersIdList[_currentProccessIndex];
                Console.WriteLine("Switch on {0} fiber with priority {1}", _currentFiberId, _processesList[_currentProccessIndex].Priority);
            }
            else
            {
                foreach (uint f in _fibersForDeleteIdList)
                {
                    if (f != Fiber.PrimaryId && f != _currentFiberId)
                        Fiber.Delete(f);
                }
                Console.WriteLine("END");
                _currentFiberId = Fiber.PrimaryId;
            }
            Thread.Sleep(100);
            Fiber.Switch(_currentFiberId);
        }
    }

    public class Process
    {
        private static readonly Random Rng = new Random();

        private const int LongPauseBoundary = 10000;

        private const int ShortPauseBoundary = 100;

        private const int WorkBoundary = 1000;

        private const int IntervalsAmountBoundary = 10;

        private const int PriorityLevelsNumber = 10;

        private readonly List<int> _workIntervals = new List<int>();
        private readonly List<int> _pauseIntervals = new List<int>();

        public Process()
        {
            int amount = Rng.Next(IntervalsAmountBoundary);

            for (int i = 0; i < amount; i++)
            {
                _workIntervals.Add(Rng.Next(WorkBoundary));
                _pauseIntervals.Add(Rng.Next(
                        Rng.NextDouble() > 0.9
                            ? LongPauseBoundary
                            : ShortPauseBoundary));
            }

            Priority = Rng.Next(PriorityLevelsNumber);
        }

        public void Run()
        {
            for (int i = 0; i < _workIntervals.Count; i++)
            {
                Thread.Sleep(_workIntervals[i]); // work emulation
                DateTime pauseBeginTime = DateTime.Now;
                do
                {
                    ProcessManager.Switch(false);
                } while ((DateTime.Now - pauseBeginTime).TotalMilliseconds < _pauseIntervals[i]); // I/O emulation
            }
            ProcessManager.Switch(true);
        }

        public int Priority
        {
            get;
            private set;
        }

        public int TotalDuration
        {
            get
            {
                return ActiveDuration + _pauseIntervals.Sum();
            }
        }

        public int ActiveDuration
        {
            get
            {
                return _workIntervals.Sum();
            }
        }
    }
}
