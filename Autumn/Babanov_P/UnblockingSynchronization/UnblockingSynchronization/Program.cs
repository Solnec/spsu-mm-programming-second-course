using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UnblockingSynchronization
{
    public class Program
    {
        static int N; //Число, факториал которого вычисляем
        static long Result = 1;
        static Mutex Lock = new Mutex();
        static bool[] EndThreads = new bool[4]; //Флаги окончания работы потоков
        public static void Main()
        {
            for(int i = 0; i < 4; i++)
            {
                EndThreads[i] = false;
            }
            string str_n = Console.ReadLine();
            N = Convert.ToInt32(str_n, 10);
            Thread Thread1 = new Thread(Program.Compute);
            Thread Thread2 = new Thread(Program.Compute);
            Thread Thread3 = new Thread(Program.Compute);
            Thread Thread4 = new Thread(Program.Compute);
            Thread1.Start(1);
            Thread2.Start(2);
            Thread3.Start(3);
            Thread4.Start(4);
            while((!EndThreads[0]) || (!EndThreads[1]) || (!EndThreads[2]) || (!EndThreads[3]))
            {
                Thread.Sleep(0);
            }
            Console.WriteLine(Result);
            Console.ReadLine();
        }

        public static void Compute(object obj_r)
        {
            int result = 1;
            int int_r = Convert.ToInt32(obj_r);
            for(int i = int_r; i <= N; i = i + 4)
            {
                result = result * i;
            }
            Lock.WaitOne();
            Result = Result * result;
            Lock.ReleaseMutex();
            EndThreads[int_r - 1] = true;
        }
    }
}
