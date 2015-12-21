using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnBlockingSynch
{
    class Factorial
    {
        UInt64 N = 0;
        UInt64[] part = new UInt64[4];
        bool[] wait = new bool[4];

        public Factorial(UInt64 N)
        {
            this.N = N;
            for (UInt64 i = 0; i < 4; i++)
            {
                part[i] = (i + 1) * N / 4;
                Console.WriteLine("part[{1}] = {0}", part[i], i);
            }
        }

        public UInt64 Result()
        {
            UInt64 Result = 1;
            Task<UInt64>[] t = new Task<UInt64>[4];
            t[0] = new Task<UInt64>(OneFact);
            t[1] = new Task<UInt64>(TwoFact);
            t[2] = new Task<UInt64>(ThreeFact);
            t[3] = new Task<UInt64>(FourFact);
            for (int i = 0; i < 4; i++)
                t[i].Start();

            while ((wait[0]) || (wait[1]) || (wait[2]) || (wait[3]))
                Thread.Sleep(0);

            for (UInt64 i = 0; i < 4; i++)
            {
                Result *= t[i].Result;
            }

            return Result;
        }

        private UInt64 OneFact()
        {
            UInt64 Result1 = 1;
            for (UInt64 i = 1; i <= part[0]; i++)
            {
                Result1 *= i;
            }
            wait[0] = false;
            return Result1;
        }

        private UInt64 TwoFact()
        {
            UInt64 Result2 = part[0] + 1;
            for (UInt64 i = Result2 + 1; i <= part[1]; i++)
            {
                Result2 *= i;
            }
            wait[1] = false;
            return Result2;
        }

        private UInt64 ThreeFact()
        {
            UInt64 Result3 = part[1] + 1;
            for (UInt64 i = Result3 + 1; i <= part[2]; i++)
            {
                Result3 *= i;
            }
            wait[2] = false;
            return Result3;
        }

        private UInt64 FourFact()
        {
            UInt64 Result4 = part[2] + 1;
            for (UInt64 i = Result4 + 1; i <= part[3]; i++)
            {
                Result4 *= i;
            }
            wait[3] = false;
            return Result4;
        }
    }
}
