using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StressTesting
{
    public class SortService: ISortService
    {
        public int[] Sort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
		        for (int j = (i % 2 == 0) ? 1 : 0; j < array.Length - 1; j += 2)
                {
			        if (array[j] > array[j + 1])
                    {
                        Swap(ref array[j] ,ref array[j + 1]);
			        }
		        }
	        }
            return array;
        }

        private void Swap(ref int i, ref int j)
        {
            int tmp = i;
            i = j;
            j = tmp;
        }
    }
}
