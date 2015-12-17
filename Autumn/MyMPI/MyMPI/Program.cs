using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace MPIService
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IService> cf = new ChannelFactory<IService>(new NetTcpBinding(), "net.tcp://127.0.0.1:1234/");
            bool IsAlive = true;

            while (IsAlive)
            {
                bool result = false;
                int Length = 0;
                while ((!result) || (Length <= 0))
                {
                    Console.WriteLine("Введите число элементов массива");
                    result = Int32.TryParse(Console.ReadLine(), out Length);
                }

                int[] A = new int[Length];

                Random rnd = new Random();
                for (int i = 0; i < Length; i++)
                {
                    A[i] = rnd.Next(-50, 50);
                    Console.Write("{0} ", A[i]);
                }

                IService channel = cf.CreateChannel();
                try
                {
                    A = channel.Sort(A);
                    Console.WriteLine("\nНовый массив: ");
                    for (int i = 0; i < Length; i++)
                    {
                        Console.Write("{0} ", A[i]);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Что -то пошло не так! Попробуйте еще раз");
                }

                Console.WriteLine("\nНажмите Enter для выхода или любую клавишу для продолжения");
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    IsAlive = false;
                    A = channel.Sort(new int[0]);
                }
            }
        }
    }
}
