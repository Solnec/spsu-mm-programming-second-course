using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;
using MPI;

namespace MPIService
{
    public class Service : IService
    {
        public static int IsAlive
        {
            get { return IsAliveServer; }
        }

        private static int IsAliveServer = 1;

        private int[] InsertSort(int[] A)
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

        private int[] CompareAndSplit(int[] A, int[] B)
        {
            int[] C = new int[A.Length + B.Length];
            int a = 0;
            int b = 0;
            int k = 0;

            while ((a < A.Length) && (b < B.Length))
            {
                if (A[a] < B[b])
                {
                    C[k] = A[a];
                    a++;
                }
                else
                {
                    C[k] = B[b];
                    b++;
                }
                k++;
            }

            for (int i = a + b; (a < A.Length) && (i < C.Length); i++, a++)
            {
                C[i] = A[a];
            }

            for (int i = b + a; (b < B.Length) && (i < C.Length); i++, b++)
            {
                C[i] = B[b];
            }
            return C;
        }

        public int[] Sort(int[] A)
        {
            var world = Communicator.world;
            int rank = world.Rank;
            int size = world.Size;

            if (rank == 0)
            {
                if (A.Length == 0)
                {
                    for (int i = 1; i < size; i++)
                    {
                        world.Send<int>(-1, i, 41);
                    }
                    Interlocked.Exchange(ref IsAliveServer, 0);
                }
                else
                {
                    int LengthForOne = A.Length / (size - 1);

                    for (int i = 1; i < size; i++)
                    {
                        world.Send<int>(LengthForOne, i, 41);
                        for (int j = 0; j < LengthForOne; j++)
                        {
                            world.Send<int>(A[(i - 1) * LengthForOne + j], i, 42);
                        }
                    }

                    int[] M = new int[LengthForOne];
                    for (int i = 0; i < LengthForOne; i++)
                    {
                        M[i] = world.Receive<int>(1, 45);
                    }

                    for (int i = 2; i < size; i++)
                    {
                        int[] N = new int[LengthForOne];

                        for (int j = 0; j < LengthForOne; j++)
                        {
                            N[j] = world.Receive<int>(i, 45);
                        }
                        M = CompareAndSplit(M, N);
                    }

                    if (A.Length % (size - 1) != 0)
                    {
                        int[] B = new int[A.Length % (size - 1)];

                        for (int i = LengthForOne * (size - 1), j = 0; ((i < A.Length) && (j < B.Length)); i++, j++)
                        {
                            B[j] = A[i];
                        }
                        B = InsertSort(B);

                        M = CompareAndSplit(M, B);
                    }

                    A = M;
                }
            }

            else
            {
                int Length = world.Receive<int>(0, 41);
                if (Length != -1)
                {
                    List<int> D = new List<int>();

                    for (int i = 0; i < Length; i++)
                        D.Add(world.Receive<int>(0, 42));
                    int[] arrD = InsertSort(D.ToArray());

                    for (int i = 0; i < arrD.Length; i++)
                    {
                        world.Send<int>(arrD[i], 0, 45);
                    }
                }
                else
                    Interlocked.Exchange(ref IsAliveServer, 0);
            }
            return A;
        }
    }
}
