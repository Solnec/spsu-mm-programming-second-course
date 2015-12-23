using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace Fibers
{
    public static class ProcessManager
    {
        static List<uint> IdFibers = new List<uint>();
        static List<uint> IdFinishedFibers = new List<uint>();
        static List<Process> Processes = new List<Process>();
        static int CurrentFiberId = 0;
        static List<int> MaxPrioruty = new List<int>();

         public static void Switch(bool fiberFinished)
         {
             if(fiberFinished)
             {
                 uint fiber = IdFibers[CurrentFiberId];
                 Console.WriteLine("Fiber {0} ended", fiber);
                 if(fiber != Fiber.PrimaryId)
                 {
                     IdFinishedFibers.Add(fiber);
                     IdFibers.RemoveAt(CurrentFiberId);
                     Processes.RemoveAt(CurrentFiberId);
                     if(IdFibers.Count > 0)
                     {
                         int max = 0;
                         if(MaxPrioruty.Count == 0)
                         {
                             for (int i = 0; i < IdFibers.Count; i++)
                             {
                                 if (Processes[i].Priority == max)
                                 {
                                     MaxPrioruty.Add(i);
                                 }
                                 if (Processes[i].Priority > max)
                                 {
                                     max = Processes[i].Priority;
                                     CurrentFiberId = i;
                                     MaxPrioruty.Clear();
                                     MaxPrioruty.Add(i);
                                 }
                             }
                         }
                         Random rnd = new Random();
                         CurrentFiberId = rnd.Next(MaxPrioruty.Count - 1);
                     }
                 }
             }
             else
             {
                 if (IdFibers.Count > 0)
                 {
                     if (MaxPrioruty.Count == 0)
                     {
                         for (int i = 0; i < IdFibers.Count; i++)
                         {
                             int max = 0;
                             if (Processes[i].Priority == max)
                             {
                                 MaxPrioruty.Add(i);
                             }
                             if (Processes[i].Priority > max)
                             {
                                 max = Processes[i].Priority;
                                 CurrentFiberId = i;
                                 MaxPrioruty.Clear();
                                 MaxPrioruty.Add(i);
                             }
                         }
                     }
                     Random rnd = new Random();
                     CurrentFiberId = rnd.Next(MaxPrioruty.Count - 1);
                 }
             }
             if(IdFibers.Count > 0)
             {
                 uint tmp = IdFibers[CurrentFiberId];
                 Fiber.Switch(tmp);
             }
             else
             {
                 Console.WriteLine("All fibers ended work");
                 Fiber.Switch(Fiber.PrimaryId);
                 foreach(uint n in IdFinishedFibers)
                 {
                     Fiber.Delete(n);
                 }
                 Fiber.Delete(Fiber.PrimaryId);
             }
        /*
        public static void Switch(bool fiberFinished)
        {
            if(fiberFinished)
            {
                uint fiber = IdFibers[CurrentFiberId];
                Console.WriteLine("Fiber {0} ended", fiber);
                if(fiber != Fiber.PrimaryId)
                {
                    IdFinishedFibers.Add(fiber);
                    IdFibers.RemoveAt(CurrentFiberId);
                    Processes.RemoveAt(CurrentFiberId);
                    if(IdFibers.Count > 0)
                    {
                        Random rnd = new Random();
                        CurrentFiberId = rnd.Next(IdFibers.Count);
                    }
                }
            }
            else
            {
                if (IdFibers.Count > 0)
                {
                    Random rnd = new Random();
                    CurrentFiberId = rnd.Next(IdFibers.Count);
                }
            }
            if(IdFibers.Count > 0)
            {
                uint tmp = IdFibers[CurrentFiberId];
                Fiber.Switch(tmp);
            }
            else
            {
                Console.WriteLine("All fibers ended work");
                Fiber.Switch(Fiber.PrimaryId);
                foreach(uint n in IdFinishedFibers)
                {
                    Fiber.Delete(n);
                }
                Fiber.Delete(Fiber.PrimaryId);
            }*/
            
            // a place for fiber magic
        }

        static void Main()
        {
            for(int i = 0; i < 8; i++)
            {
                Process process = new Process();
                Processes.Add(process);
                Fiber fiber = new Fiber(new Action(process.Run));
                IdFibers.Add(fiber.Id);
            }
            Switch(false);
            Console.ReadLine();
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
