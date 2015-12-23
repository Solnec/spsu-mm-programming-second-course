using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace ProcessManager
{
    public static class ProcessManager
    {
        public static List<Process> ListOfProcesses = new List<Process>();
        public static List<uint> ListOfFibers = new List<uint>();
        private static int IndexOfMaxPriority;

        //Priority
        //public static void Switch(bool fiberFinished)
        //{
        //    Thread.Sleep(100);
        //
        //    if (!fiberFinished)
        //    {
        //        for (int i = 10; i >= 0; i--)
        //        {
        //            List<Process> maxPriority = ListOfProcesses.FindAll(element => element.Priority == i);
        //            if (maxPriority.Count == 0) continue;
        //            else
        //            {
        //                IndexOfMaxPriority = ListOfProcesses.FindIndex(element => element.Priority == i);
        //                break;
        //            }
        //        }
        //    }
        //
        //    else
        //    {
        //        if (ListOfFibers[IndexOfMaxPriority] != Fibers.Fiber.PrimaryId)
        //        {
        //            ListOfProcesses.RemoveAt(IndexOfMaxPriority);
        //            ListOfFibers.RemoveAt(IndexOfMaxPriority);
        //            IndexOfMaxPriority = 0;
        //        }
        //    }
        //
        //    if (ListOfFibers.Count != 0) Fibers.Fiber.Switch(ListOfFibers[IndexOfMaxPriority]);
        //    else
        //    {
        //        Console.WriteLine("All fibers finished their working.");
        //        Fibers.Fiber.Switch(Fibers.Fiber.PrimaryId);
        //        foreach (uint Fiber in ListOfFibers) Fibers.Fiber.Delete(Fiber);
        //        Fibers.Fiber.Delete(Fibers.Fiber.PrimaryId);
        //    }
        //}

        //Unpriority
        public static void Switch(bool fiberFinished)
        {
            Thread.Sleep(100);

            if (!fiberFinished)
            {
                IndexOfMaxPriority = 0;
            }

            else
            {
                if (ListOfFibers[IndexOfMaxPriority] != Fibers.Fiber.PrimaryId)
                {
                    ListOfProcesses.RemoveAt(IndexOfMaxPriority);
                    ListOfFibers.RemoveAt(IndexOfMaxPriority);
                    IndexOfMaxPriority = 0;
                }
            }

            if (ListOfFibers.Count != 0)
            {
                Fibers.Fiber.Switch(ListOfFibers[IndexOfMaxPriority]);
            }
            else
            {
                Console.WriteLine("All fibers finished their working.");
                Fibers.Fiber.Switch(Fibers.Fiber.PrimaryId);
                foreach (uint Fiber in ListOfFibers) Fibers.Fiber.Delete(Fiber);
                Fibers.Fiber.Delete(Fibers.Fiber.PrimaryId);
            }
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
			get; private set;
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
