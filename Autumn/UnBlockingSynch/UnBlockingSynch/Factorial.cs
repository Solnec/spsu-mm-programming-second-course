using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

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
                wait[i] = true;
                part[i] = (i + 1) * N / 4;
            }
        }

        public BigInteger Result()
        {
            BigInteger  Result = 1;
            Task<BigInteger>[] t = new Task<BigInteger>[4];
            t[0] = new Task<BigInteger>(OneFact);
            t[1] = new Task<BigInteger>(TwoFact);
            t[2] = new Task<BigInteger>(ThreeFact);
            t[3] = new Task<BigInteger>(FourFact);
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

        private BigInteger OneFact()
        {
            BigInteger Result1 = 1;
            for (BigInteger i = 1; i <= part[0]; i++)
            {
                Result1 *= i;
            }
            wait[0] = false;
            return Result1;
        }

        private BigInteger TwoFact()
        {
            BigInteger Result2 = part[0] + 1;
            for (BigInteger i = Result2 + 1; i <= part[1]; i++)
            {
                Result2 *= i;
            }
            wait[1] = false;
            return Result2;
        }

        private BigInteger ThreeFact()
        {
            BigInteger Result3 = part[1] + 1;
            for (BigInteger i = Result3 + 1; i <= part[2]; i++)
            {
                Result3 *= i;
            }
            wait[2] = false;
            return Result3;
        }

        private BigInteger FourFact()
        {
            BigInteger Result4 = part[2] + 1;
            for (BigInteger i = Result4 + 1; i <= part[3]; i++)
            {
                Result4 *= i;
            }
            wait[3] = false;
            return Result4;
        }
    }
}
