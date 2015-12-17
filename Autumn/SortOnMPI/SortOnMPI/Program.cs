using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MPI;
using Sort;

namespace SortOnMPI
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {

                int[] arr = { 100, 86, 54, 75, 12, 64, 32, 19, 48, 76, 51, 87, 16, 24, 81, 42, 36, 99, 1, -1, 1000000, 0, 0, 1000000, 703, 2 };
                //int[] arr = { 4, 3, 2, 1 };
                Sort.Sort.SortArray(arr, args);
                if (Communicator.world.Rank == 0)
                {
                    foreach (var i in arr)
                    {
                        Console.WriteLine(i);
                    }
                }
            }
        }
    }
}
