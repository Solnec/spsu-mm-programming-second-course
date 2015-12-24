using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormFilters;

namespace GraphicFilters
{
    class Filters
    {
        public int CountOfIteration;

        private void matrixProcessing(BMPFile procBMP, BMPFile interBMP, int m, double[,] convolMatrix)
        {
            double intermediateB = .0;
            double intermediateG = .0;
            double intermediateR = .0;

            for (int i = 1; i < interBMP.biHeight - m + 2; i++)
            {
                Form1.Filter.Set();

                for (int j = 1; j < interBMP.biWidth - m + 2; j++)
                {
                    for (int k = 0; k < m; k++)
                        for (int l = 0; l < m; l++)
                        {
                            intermediateB += procBMP.colours[i + k - 1, j + l - 1].B * convolMatrix[k, l];
                            intermediateG += procBMP.colours[i + k - 1, j + l - 1].G * convolMatrix[k, l];
                            intermediateR += procBMP.colours[i + k - 1, j + l - 1].R * convolMatrix[k, l];
                        }

                    interBMP.colours[i, j].B = (byte)intermediateB;
                    interBMP.colours[i, j].G = (byte)intermediateG;
                    interBMP.colours[i, j].R = (byte)intermediateR;

                    intermediateB = .0;
                    intermediateG = .0;
                    intermediateR = .0;
                }

                CountOfIteration = i;

                Form1.Event.Set();
                Form1.Filter.WaitOne();
            }
        }

        public BMPFile Grayscale(BMPFile BMP)
        {
            BMPFile newBMP = new BMPFile();
            newBMP.Copy(BMP);

            for (int i = 0; i < newBMP.biHeight; i++)
            {
                Form1.Filter.Set();

                for (int j = 0; j < newBMP.biWidth; j++)
                    newBMP.colours[i, j].B =
                    newBMP.colours[i, j].G =
                    newBMP.colours[i, j].R =
                    (byte)((.0722 * BMP.colours[i, j].B + .7152 * BMP.colours[i, j].G + .2126 * BMP.colours[i, j].R));
                
                CountOfIteration = i;

                Form1.Event.Set();
                Form1.Filter.WaitOne();
            }

            return newBMP;
        }

        public BMPFile Gauss(BMPFile BMP)
        {
            BMPFile newBMP = new BMPFile();
            newBMP.Copy(BMP);

            double[,] matrixGauss = new double[5, 5] {{ .000789, .006581, .013347, .006581, .000789}, 
                                                      { .006581, .054901, .111345, .054901, .006581},
                                                      { .013347, .111345, .225821, .111345, .013347},
                                                      { .006581, .054901, .111345, .054901, .006581},
                                                      { .000789, .006581, .013347, .006581, .000789}};

            matrixProcessing(BMP, newBMP, 5, matrixGauss);

            return newBMP;
        }

        public BMPFile Average3x3(BMPFile BMP)
        {
            BMPFile newBMP = new BMPFile();
            newBMP.Copy(BMP);

            double[,] averMatrix = new double[3, 3] { { .111111, .111111, .111111 }, 
                                                      { .111111, .111111, .111111 }, 
                                                      { .111111, .111111, .111111 } };

            matrixProcessing(BMP, newBMP, 3, averMatrix);

            return newBMP;
        }

        public BMPFile Average5x5(BMPFile BMP)
        {
            BMPFile newBMP = new BMPFile();
            newBMP.Copy(BMP);

            double[,] averMatrix = new double[5, 5] { { .04, .04, .04, .04, .04 }, 
                                                      { .04, .04, .04, .04, .04 }, 
                                                      { .04, .04, .04, .04, .04 }, 
                                                      { .04, .04, .04, .04, .04 }, 
                                                      { .04, .04, .04, .04, .04 } };

            matrixProcessing(BMP, newBMP, 5, averMatrix);

            return newBMP;
        }

        public BMPFile SobelX(BMPFile BMP)
        {
            BMPFile newBMP = new BMPFile();
            newBMP.Copy(BMP);

            int[,] sobelXMatrix = new int[3, 3] { { -1, 0, 1 }, 
                                                  { -2, 0, 2 }, 
                                                  { -1, 0, 1 } };

            double[,] intensity = new double[newBMP.biHeight, newBMP.biWidth];

            for (int i = 0; i < newBMP.biHeight; i++)
                for (int j = 0; j < newBMP.biWidth; j++)
                    intensity[i, j] = .0722 * newBMP.colours[i, j].B + .7152 * newBMP.colours[i, j].G + .2126 * newBMP.colours[i, j].R;

            for (int i = 1; i < newBMP.biHeight - 1; i++)
            {
                Form1.Filter.Set();

                for (int j = 1; j < newBMP.biWidth - 1; j++)
                {
                    double intermediateB = 0;

                    for (int k = 0; k < 3; k++)
                        for (int l = 0; l < 3; l++)
                            intermediateB += intensity[i + k - 1, j + l - 1] * sobelXMatrix[k, l];

                    newBMP.colours[i, j].B = newBMP.colours[i, j].G = newBMP.colours[i, j].R = (byte)(Math.Abs(intermediateB) * .255);
                }

                CountOfIteration = i;

                Form1.Event.Set();
                Form1.Filter.WaitOne();
            }

            return newBMP;
        }

        public BMPFile SobelY(BMPFile BMP)
        {
            BMPFile newBMP = new BMPFile();
            newBMP.Copy(BMP);

            int[,] sobelXMatrix = new int[3, 3] { { -1, -2, -1 }, 
                                                  { 0, 0, 0 }, 
                                                  { 1, 2 , 1 } };

            double[,] intensity = new double[newBMP.biHeight, newBMP.biWidth];

            for (int i = 0; i < newBMP.biHeight; i++)
                for (int j = 0; j < newBMP.biWidth; j++)
                    intensity[i, j] = .0722 * newBMP.colours[i, j].B + .7152 * newBMP.colours[i, j].G + .2126 * newBMP.colours[i, j].R;

            for (int i = 1; i < newBMP.biHeight - 1; i++)
            {
                Form1.Filter.Set();

                for (int j = 1; j < newBMP.biWidth - 1; j++)
                {
                    double intermediateB = 0;

                    for (int k = 0; k < 3; k++)
                        for (int l = 0; l < 3; l++)
                            intermediateB += intensity[i + k - 1, j + l - 1] * sobelXMatrix[k, l];

                    newBMP.colours[i, j].B = newBMP.colours[i, j].G = newBMP.colours[i, j].R = (byte)(Math.Abs(intermediateB) * .255);
                }

                CountOfIteration = i;

                Form1.Event.Set();
                Form1.Filter.WaitOne();
            }

            return newBMP;
        }
    }
}
