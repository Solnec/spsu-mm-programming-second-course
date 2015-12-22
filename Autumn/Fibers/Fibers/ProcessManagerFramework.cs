using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace Fibers
{
    public static class ProcessManager
    {
        public static List<Process> Processes = new List<Process>();
        public static List<uint> Fibers = new List<uint>();
        public static List<uint> FibersForDelete = new List<uint>();
        public static int CurrentProccess;
        static uint CurrentFiber;

        private static void ShiftQueue()
        {
            uint A = Fibers[Fibers.Count - 1];
            Process proc = Processes[Processes.Count - 1];

            for (int i = Fibers.Count - 1; i > 0; i--)
            {
                Processes[i] = Processes[i - 1];
                Fibers[i] = Fibers[i - 1];
            }
            Fibers[0] = A;
            Processes[0] = proc;
        }

        private static void GetMaxPriority()
        {
            Process PriorityProcess = new Process();
            int MaxPriority = Int32.MinValue;

            foreach (Process proccess in Processes)
            {
                if ((MaxPriority < proccess.Priority) && (proccess != Processes[CurrentProccess]))
                {
                    MaxPriority = proccess.Priority;
                    PriorityProcess = proccess;
                }
            }
            CurrentProccess = Processes.IndexOf(PriorityProcess);
        }

        //Unpriority dispatch
        public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                if (CurrentFiber != Fiber.PrimaryId)
                {
                    uint FiberForDelete = CurrentFiber;
                    Fibers.Remove(FiberForDelete);
                    Processes.Remove(Processes[CurrentProccess]);
                    CurrentProccess = 0;
                }
            }

            if (Fibers.Count > 1)
            {
                ShiftQueue();
                CurrentFiber = Fibers[Fibers.Count - 1];
                Console.WriteLine("Switch on another fiber");
            }
            else
            {
                foreach (uint f in FibersForDelete)
                {
                    if ((f != Fiber.PrimaryId) && (f != CurrentFiber))
                    {
                        Console.WriteLine("Delete fiber{0}", f);
                        Fiber.Delete(f);
                    }
                }
                Console.WriteLine("All fibers are deleted");
                CurrentFiber = Fiber.PrimaryId;
            }
            Fiber.Switch(CurrentFiber);
        }

        //Priority dispatch
        /*  public static void Switch(bool fiberFinished)
         {
             if (fiberFinished)
             {
                 if (CurrentFiber != Fiber.PrimaryId)
                 {
                     uint FiberForDelete = CurrentFiber;
                     Fibers.Remove(FiberForDelete);
                     Processes.Remove(Processes[CurrentProccess]);
                     CurrentProccess = 0;
                 }
             }

             if (Fibers.Count > 1)
             {
                 GetMaxPriority();
                 CurrentFiber = Fibers[CurrentProccess];
                 Console.WriteLine("Switch on another fiber with priority {0}", Processes[CurrentProccess].Priority);
             }
             else
             {
                 foreach (uint f in Fibers)
                 {
                     if (f != Fiber.PrimaryId)
                         Fiber.Delete(f);
                 }
                 Console.WriteLine("All fibers are deleted");
                 CurrentFiber = Fiber.PrimaryId;
             }
             Thread.Sleep(100);           
             Fiber.Switch(CurrentFiber);
         }*/
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
