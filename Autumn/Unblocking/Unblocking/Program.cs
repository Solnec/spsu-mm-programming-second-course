using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;

namespace Unblocking
{
    class Program
    {
        static int n = 2015;

        static void Main()
        {
            BigInteger Result = 1;

            Thread[] threads = new Thread[4];
            BigInteger[] results = new BigInteger[4];

            threads[0] = new Thread(new ThreadStart(delegate() { results[0] = Calculating(1); }));
            threads[1] = new Thread(new ThreadStart(delegate() { results[1] = Calculating(2); }));
            threads[2] = new Thread(new ThreadStart(delegate() { results[2] = Calculating(3); }));
            threads[3] = new Thread(new ThreadStart(delegate() { results[3] = Calculating(4); }));

            threads[0].Start();
            threads[1].Start();
            threads[2].Start();
            threads[3].Start();

            while (threads[0].ThreadState == ThreadState.Running ||
                  threads[1].ThreadState == ThreadState.Running ||
                  threads[2].ThreadState == ThreadState.Running ||
                  threads[3].ThreadState == ThreadState.Running)
                Thread.Sleep(0);

            for (int i = 0; i < 4; i++)
                Result *= results[i];

            Console.WriteLine("{0}! = {1}", n, Result);
            Console.ReadLine();
        }

        private static BigInteger Calculating(int low)
        {
            BigInteger res = 1;
            for (int i = low; i <= n; i += 4)
                res *= i;
            return res;
        }
    }
}
