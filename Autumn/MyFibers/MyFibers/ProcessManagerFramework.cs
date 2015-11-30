using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

using Fibers;

namespace ProcessManager
{
    public static class ProcessManager
    {
        private static List<Process> processes = new List<Process>();
        private static List<uint> fibers = new List<uint>();
        private static List<uint> finished = new List<uint>();
        private static int fiberIndex = 0;

        //приоритетный алгоритм диспетчеризации
        public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                //Console.WriteLine("1");
                uint fiberId = fibers[fiberIndex];
                Console.WriteLine("Fiber [{0}] finished working", fiberId);
                if (fiberId != Fiber.PrimaryId)
                {
                    finished.Add(fiberId);
                    processes.RemoveAt(fiberIndex);
                    fibers.RemoveAt(fiberIndex);
                    if (fibers.Count > 0)
                    {
                        fiberIndex %= fibers.Count;
                    }
                }
            }
            else
            {
                //Console.WriteLine("2");
                
                Random rng = new Random();
                for (int i=0; i<processes.Count; i++)
                {
                    processes[i].Priority = rng.Next(10);
                }
                int max = 0;
                //вычисляем самый приоритетный процесс
                for (int i = 0; i < processes.Count; i++)
                {
                    if (processes[i].Priority > processes[max].Priority)
                    {
                        max = i;
                    }
                }
                for (int i = 0; i < processes.Count; i++)
                {
                    //Console.WriteLine("{0}", processes[i].Priority);
                }
                //Console.WriteLine("max = {0}", max);
                //Console.ReadLine();
              
                fiberIndex = max;
                //fiberIndex++;
                //fiberIndex %= fibers.Count;
            }
            if (fibers.Count > 0)
            {
                //Console.WriteLine("3");
                uint fiberId = fibers[fiberIndex];
                Fiber.Switch(fiberId);
            }
            else
            {
                Console.WriteLine("All fibers finished working");
                Fiber.Switch(Fiber.PrimaryId);
                foreach (uint fiberId in finished)
                {
                    Fiber.Delete(fiberId);
                }
                Fiber.Delete(Fiber.PrimaryId);
                
                //processes.Clear();
            }
        }

        //бесприоритетный алгоритм диспетчеризации
        /*public static void Switch(bool fiberFinished)
        {
            if (fiberFinished)
            {
                uint fiberId = fibers[fiberIndex];
                Console.WriteLine("Fiber [{0}] finished working", fiberId);
                if (fiberId != Fiber.PrimaryId)
                {
                    finished.Add(fiberId);
                    fibers.RemoveAt(fiberIndex);
                }
            }
            else
            {
                fiberIndex++;
            }
            if (fibers.Count > 0)
            {
                fiberIndex %= fibers.Count;
                uint fiberId = fibers[fiberIndex];
                Fiber.Switch(fiberId);
            }
            else
            {
                Console.WriteLine("All fibers finished working");
                Fiber.Switch(Fiber.PrimaryId);
                foreach (uint fiberId in finished)
                {
                    Fiber.Delete(fiberId);
                }
                Fiber.Delete(Fiber.PrimaryId);
            }
        }*/


        public static void Main()
        {
            for (int i = 0; i < 4; i++)
            {
                Process process = new Process();
                processes.Add(process);
                Fiber fiber = new Fiber(new Action(process.Run));
                fibers.Add(fiber.Id);
            }
            Switch(false);
            Console.ReadLine();
        }
    }

    public class Process
    {
        static readonly Random Rng = new Random();

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
			get; set;
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
