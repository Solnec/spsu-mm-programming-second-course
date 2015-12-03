using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyWCFService
{
    public class Service : IService
    {
        public int[] Sort(int[] A)
        {
            for (int j = 1; j < A.Length; j++)
            {
                int key = A[j];
                int i = j - 1;
                while ((i >= 0) && (A[i] > key))
                {
                    A[i + 1] = A[i];
                    i--;
                }
                A[i + 1] = key;
            }
            return A;
        }
    }
}
