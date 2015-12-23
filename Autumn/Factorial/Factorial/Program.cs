using System;
using System.Numerics;
using System.Collections.Generic;
using System.Threading;

namespace Factorial
{
    static class Program
    {
        public static BigInteger TotalResult = 1;
        public static int NumberOfFinishedThreads;

        static void Main()
        {
            //get number to check the factorial on
            Console.WriteLine("Please enter the number to calculate the factorial on: ");
            int number = int.Parse(Console.ReadLine());

            //create a list to hold all the threads
            var threadList = new List<Thread>();
            for (int i = 0; i < 4; i++)
            {
                var partFactorial = new Factorial();
                threadList.Add(new Thread(partFactorial.CalculateTheProductFromNumberToMunber));
            }

            var twoNumbersList = new List<TwoNumbers>();
            var twoNumbers = new TwoNumbers { FirstNumber = 1 };
            int jumpSize = number / 4;
            twoNumbers.LastNumber = jumpSize;

            for (int i = 0; i < 4; i++)
            {
                twoNumbersList.Add(new TwoNumbers(twoNumbers.FirstNumber, twoNumbers.LastNumber));
                twoNumbers.FirstNumber = twoNumbers.LastNumber + 1;
                if ((i + 2) < 4)
                    twoNumbers.LastNumber += jumpSize;
                else
                    twoNumbers.LastNumber = number;
            }

            for (int i = 0; i < 4; i++)
            {
                threadList[i].Start(twoNumbersList[i]);
            }

            while (NumberOfFinishedThreads < 4)
            {
                Thread.Sleep(300);
            }
            Console.WriteLine("The result is {0}", TotalResult);
            Console.ReadLine();
        }
    }

    public class TwoNumbers
    {
        public TwoNumbers()
        {
        }
        public TwoNumbers(int first, int seconed)
        {
            FirstNumber = first;
            LastNumber = seconed;
        }
        public int FirstNumber { get; set; }
        public int LastNumber { get; set; }
    }
}