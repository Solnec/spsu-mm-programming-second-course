using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sort;

namespace SortOnMPI
{
    class Program
    {
        private static void Main(string[] args)
        {
            int[] arr = { 100, 86, 54, 75, 12, 64, 32, 19, 48, 76, 51, 87, 16, 24, 81, 42, 36, 99, 1, -1 };
            //int[] arr = { 4, 3, 2, 1 };
            Sort.Sort.SortArray(arr, args);
            foreach (var i in arr)
            {
                Console.WriteLine(i);
            }
        }
    }
}
