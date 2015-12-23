using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Non_Blocking_Algorithm
{
    class Program
    {
        static int input;
        static int factorial;
        static int[] currentF;
        static bool[] currentR;
        static void Main(string[] args)
        {
            factorial = 1;
            Console.WriteLine("Введите число, факториал которого хотите найти.");
            input = Convert.ToInt32(Console.ReadLine());

            currentF = new int[] {1, 1, 1, 1};
            currentR = new bool[] { false, false, false, false };

            var threadOne = new Thread(ThreadFuncOne);
            var threadTwo = new Thread(ThreadFuncTwo);
            var threadThree = new Thread(ThreadFuncThree);
            var threadFour = new Thread(ThreadFuncFour);

            threadOne.Start(input);
            threadTwo.Start(input);
            threadThree.Start(input);
            threadFour.Start(input);


            while(!currentR[0] || !currentR[1] || !currentR[2] || !currentR[3])
            {

            }
            factorial = currentF[0] * currentF[1] * currentF[2] * currentF[3];
            Console.WriteLine("Факториал числа {0} - {1}.", input, factorial);
            Console.ReadKey();
        }

        static void ThreadFuncOne(object number)
        {
            int counter = 1;
            while(counter <= (int)number)
            {
                currentF[0] *= counter;
                counter += 4;
            }

            currentR[0] = true;
        }

        static void ThreadFuncTwo(object number)
        {
            int counter = 2;
            while (counter <= (int)number)
            {
                currentF[1] *= counter;
                counter += 4;
            }

            currentR[1] = true;
        }

        static void ThreadFuncThree(object number)
        {
            int counter = 3;
            while (counter <= (int)number)
            {
                currentF[2] *= counter;
                counter += 4;
            }

            currentR[2] = true;
        }

        static void ThreadFuncFour(object number)
        {
            int counter = 4;
            while (counter <= (int)number)
            {
                currentF[3] *= counter;
                counter += 4;
            }

            currentR[3] = true;
        }
    }
}
