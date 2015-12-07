using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WCFService
{
    class Service:IService
    {
        public int[,] MatrixMultiplication(int[,] m1, int[,] m2)
        {
            if(m1.GetLength(1) != m2.GetLength(0)) //0 - строки 1 - столбцы
                return null;
            int columns, line, overallNumbers;
            line = m1.GetLength(0);
            columns = m2.GetLength(1);
            overallNumbers = m1.GetLength(1);
            int[,] result = new int[line, columns];
            Console.WriteLine("Start Work");
            for (int i = 0; i < line; i++)
            {
                for(int j = 0; j<columns; j++)
                {
                    int number = 0;
                    for(int k=0; k<overallNumbers;k++)
                    {
                        number = number + m1[i, k] * m2[k, j];
                    }
                    result[i, j] = number;
                }
            }
            Console.WriteLine("End Work");
            return result;
        }
    }
}
