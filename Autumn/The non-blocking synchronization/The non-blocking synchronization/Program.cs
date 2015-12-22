using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace The_non_blocking_synchronization
{
    class Program
    {
        static int count = 4;
        static Task<BigInteger>[] tasks = new Task<BigInteger>[count];

        static void Main(string[] args)
        {            
            Random rng = new Random();
            int n = rng.Next(1000); 
            int range = n / count;
            BigInteger result = 1;           
        
            Console.WriteLine("Calculated factorial({0}) ...", n);

            for (int i = 0; i < count; i++)
            {
                int start = i * range + 1;
                int end = (i == count - 1) ? n : start + range - 1;
                tasks[i] = new Task<BigInteger>(() => work(start, end));
                tasks[i].Start();
            }

            Task.WaitAll(tasks);
            for (int i = 0; i < count; i++) result *= tasks[i].Result;

            Console.WriteLine("Factorial({0})={1}", n, result);
            Console.ReadLine();
        }

        static BigInteger work(int start, int end)
        {
            BigInteger result = 1;
            for (int i = start; i <= end; i++) result *= i;
            return result;
        }
    }
}
