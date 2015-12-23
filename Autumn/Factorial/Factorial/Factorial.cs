 using System;
using System.Numerics;
 
 namespace Factorial
{
     class Factorial
     {        
         public void CalculateTheProductFromNumberToMunber(object input)
         {
             var twoNumbers = (TwoNumbers)input;
             BigInteger result = 1;
             for (int i = twoNumbers.FirstNumber; i <= twoNumbers.LastNumber; i++)
             {
                 result *= i;
             }
             Program.TotalResult *= result;      
             Program.NumberOfFinishedThreads += 1;
         }
     }
}