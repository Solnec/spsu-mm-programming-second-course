using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

namespace NonBlockingFactorial
{
    static class Factorial
    {
        private static BigInteger _value;
        private static BigInteger _result = 0;
        private static BigInteger _multOne = 1;
        private static BigInteger _multTwo = 1;
        private static BigInteger _multThree = 1;
        private static BigInteger _multFour = 1;
        private static int _endedThreads = 0;
        private static bool _isWorked = true;

        public static BigInteger Fact(int value)
        {
            _value = value;
            _isWorked = true;
            ThreadPool.QueueUserWorkItem(FactorialOne);
            ThreadPool.QueueUserWorkItem(FactorialTwo);
            ThreadPool.QueueUserWorkItem(FactorialThree);
            ThreadPool.QueueUserWorkItem(FactorialFour);
            while (_isWorked)
            {
                Thread.Sleep(0);
            }
            return _result;
        }

        private static void FactorialOne(object o)
        {
            _multOne = 1;
            for (int i = 1; i <= _value; i+=4)
            {
                _multOne *= i;
            }
            Interlocked.Increment(ref _endedThreads);
            FactorialLast();
        }
        private static void FactorialTwo(object o)
        {
            _multTwo = 1;
            for (int i = 2; i <= _value; i += 4)
            {
                _multTwo *= i;
            }
            Interlocked.Increment(ref _endedThreads);
            FactorialLast();
        }
        private static void FactorialThree(object o)
        {
            _multThree = 1;
            for (int i = 3; i <= _value; i += 4)
            {
                _multThree *= i;
            }
            Interlocked.Increment(ref _endedThreads);
            FactorialLast();
        }
        private static void FactorialFour(object o)
        {
            _multFour = 1;
            for (int i = 4; i <= _value; i += 4)
            {
                _multFour *= i;
            }
            Interlocked.Increment(ref _endedThreads);
            FactorialLast();
        }

        private static void FactorialLast()
        {
            int tmp = Interlocked.CompareExchange(ref _endedThreads, 0, 4);
            if (tmp == 4)
            {
                _result = _multOne*_multTwo*_multThree*_multFour;
                _isWorked = false;
            }
        }





    }
}
