using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace Sort
{
    public static class Sort
    {
        public static void SortArray(int[] arr, string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                int rank = comm.Rank;
                int size = comm.Size;
                if (size == 1)
                    return;
                int sizeTwo = (size - 1) * 2;
                if (arr.Count() < sizeTwo)
                    return;

                int sizeOfBlock = arr.Count() / sizeTwo;
                int numberOfMod = arr.Count() % sizeTwo;
                int[][] blocks = new int[sizeTwo][];
                for (int i = 0; i < sizeTwo - 1; i++)
                {
                    blocks[i] = new int[sizeOfBlock];
                    for (int j = 0; j < sizeOfBlock; j++)
                    {
                        blocks[i][j] = arr[i * sizeOfBlock + j];
                    }
                }
                blocks[sizeTwo - 1] = new int[sizeOfBlock + numberOfMod];

                int tmpCount = 0;
                for (int i = 0; i < blocks.Count(); i++)
                {
                    for (int j = 0; j < blocks[i].Count(); j++)
                    {
                        blocks[i][j] = arr[tmpCount];
                        tmpCount++;
                    }
                }

                if (rank == 0)
                {
                    comm.Barrier();
                    for (int i = 0; i < size - 1; i++)
                    {
                        comm.Send<int[]>(blocks[2 * i], i + 1, 0);
                        comm.Send<int[]>(blocks[2 * i + 1], i + 1, 0);
                    }
                    for (int i = 0; i < size - 1; i++)
                    {
                        blocks[2 * i] = comm.Receive<int[]>(i + 1, 0);
                        blocks[2 * i + 1] = comm.Receive<int[]>(i + 1, 0);
                    }
                    comm.Barrier();


                    for (int k = 0; k < sizeTwo; k++)
                    {
                        if (k % 2 == 0)
                        {
                            for (int i = 0; i < size - 1; i++)
                            {
                                comm.Send<int[]>(blocks[2 * i], i + 1, 0);
                                comm.Send<int[]>(blocks[2 * i + 1], i + 1, 0);
                            }
                            for (int i = 0; i < size - 1; i++)
                            {
                                blocks[2 * i] = comm.Receive<int[]>(i + 1, 0);
                                blocks[2 * i + 1] = comm.Receive<int[]>(i + 1, 0);

                            }
                        }
                        else
                        {
                            for (int i = 0; i < size - 2; i++)
                            {
                                comm.Send<int[]>(blocks[2 * i + 1], i + 1, 0);
                                comm.Send<int[]>(blocks[2 * i + 2], i + 1, 0);
                            }
                            for (int i = 0; i < size - 2; i++)
                            {
                                blocks[2 * i + 1] = comm.Receive<int[]>(i + 1, 0);
                                blocks[2 * i + 2] = comm.Receive<int[]>(i + 1, 0);
                            }
                        }
                        comm.Barrier();
                    }

                    tmpCount = 0;
                    for (int i = 0; i < blocks.Count(); i++)
                    {
                        for (int j = 0; j < blocks[i].Count(); j++)
                        {
                            arr[tmpCount] = blocks[i][j];
                            tmpCount++;
                        }
                    }
                    comm.Barrier();
                }

                if (rank != 0)
                {
                    comm.Barrier();
                    int[] blockFirst;
                    int[] blockSecond;
                    blockFirst = comm.Receive<int[]>(0, 0);
                    blockSecond = comm.Receive<int[]>(0, 0);
                    Array.Sort(blockFirst);
                    Array.Sort(blockSecond);
                    comm.Send<int[]>(blockFirst, 0, 0);
                    comm.Send<int[]>(blockSecond, 0, 0);
                    comm.Barrier();

                    for (int i = 0; i < sizeTwo; i++)
                    {
                        if (!(rank == size - 1 && i % 2 == 1))
                        {
                            blockFirst = comm.Receive<int[]>(0, 0);
                            blockSecond = comm.Receive<int[]>(0, 0);
                            MergeSplit(blockFirst, blockSecond);
                            comm.Send<int[]>(blockFirst, 0, 0);
                            comm.Send<int[]>(blockSecond, 0, 0);
                        }
                        comm.Barrier();
                    }
                    comm.Barrier();
                }
            }


        }

        static void MergeSplit(int[] first, int[] second)
        {
            int[] tmp = new int[first.Count() + second.Count()];
            int i = 0;
            int j = 0;
            int count = 0;
            while (i < first.Count() && j < second.Count())
            {
                if (first[i] < second[j])
                {
                    tmp[count] = first[i];
                    i++;
                }
                else
                {
                    tmp[count] = second[j];
                    j++;
                }
                count++;
            }

            while (i < first.Count())
            {
                tmp[count] = first[i];
                count++;
                i++;
            }
            while (j < second.Count())
            {
                tmp[count] = second[j];
                count++;
                j++;
            }

            count = 0;
            for (int k = 0; k < first.Count(); k++)
            {
                first[k] = tmp[count];
                count++;
            }
            for (int k = 0; k < second.Count(); k++)
            {
                second[k] = tmp[count];
                count++;
            }
        }
    }
}
