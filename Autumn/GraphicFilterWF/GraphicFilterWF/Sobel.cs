using System;
using System.Drawing;

namespace GraphicFilterWF
{
    class Sobel : IFilter
    {
        private static int[,] _sobelYMatrix = 
        {
            { -1,-2,-1},
            {  0, 0, 0},
            {  1, 2, 1}
        };

        private static int[,] _sobelXMatrix = 
        {
            { -1, 0, 1},
            { -2, 0, 2},
            { -1, 0, 1}
        };

        private int _axis;
        public Sobel(int axis)
        {
            _axis = axis;
        }
        public Bitmap ApplyFilter(Bitmap image)
        {
            double progress = 0;
            Bitmap newImage = new Bitmap(image);

            int[,] intensity = new int[image.Width, image.Height];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    intensity[i, j] = (image.GetPixel(i, j).R + image.GetPixel(i, j).G + image.GetPixel(i, j).B) / 3;
                    progress+= 0.5;
                    Progress = (int) progress;
                }
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    ApplyMatrix(i, j, intensity, newImage);
                    progress += 0.5;
                    Progress = (int)progress;
                }
            }
            return newImage;
        }

        public int Progress
        {
            get;
            private set;
        }

        private void ApplyMatrix(int col, int row, int[,] image, Bitmap newImage)
        {
            int sumX = 0;
            int sumY = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int X = col + ConvolutionMatrix.GetX3x3(i, j);
                    int Y = row + ConvolutionMatrix.GetY3x3(i, j);

                    if (X >= 0 && Y >= 0 && X < newImage.Width && Y < newImage.Height)
                    {
                        switch (_axis)
                        {
                            case 0:
                                sumX += (image[X, Y] * _sobelXMatrix[i, j]);
                                break;
                            case 1:
                                sumY += (image[X, Y] * _sobelYMatrix[i, j]);
                                break;
                            case 2:
                                sumX += (image[X, Y] * _sobelXMatrix[i, j]);
                                sumY += (image[X, Y] * _sobelYMatrix[i, j]);
                                break;
                        }

                    }
                }
            }
            byte tmpByte = (byte)((Math.Sqrt(Math.Pow((sumX), 2d) + Math.Pow((sumY), 2d))) * 255 / 1000);
            newImage.SetPixel(col, row, Color.FromArgb(tmpByte, tmpByte, tmpByte));
        }
    }
}
