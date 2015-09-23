using System;

namespace GraphicFilterWF
{
    class Sobel : IFilter
    {
        private static int[,] _sobelXMatrix = 
        {
            { -1, 0, 1},
            { -2, 0, 2},
            { -1, 0, 1}
        };

        private static int[,] _sobelYMatrix = 
        {
            { -1,-2,-1},
            {  0, 0, 0},
            {  1, 2, 1}
        };

        private int _axis;
        public Sobel(int axis)
        {
            _axis = axis;
        }
        public BMP ApplyFilter(BMP image)
        {
            BMP newImage = new BMP(image);

            int[,] intensity = new int[image.BiHeight, image.BiWidth];

            for (int i = 0; i < image.BiHeight; i++)
            {
                for (int j = 0; j < image.BiWidth; j++)
                {
                    intensity[i, j] = (image.Сolors[i, j].R + image.Сolors[i, j].G + image.Сolors[i, j].B) / 3;
                }
            }

            for (int i = 0; i < image.BiHeight; i++)
            {
                for (int j = 0; j < image.BiWidth; j++)
                {
                    ApplyMatrix(i, j, intensity, newImage);
                }
            }
            return newImage;
        }

        private void ApplyMatrix(int col, int row, int[,] image, BMP newImage)
        {
            int sumX = 0;
            int sumY = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int X = col + ConvolutionMatrix.GetX3x3(i, j);
                    int Y = row + ConvolutionMatrix.GetY3x3(i, j);

                    if (X >= 0 && Y >= 0 && X < newImage.BiHeight && Y < newImage.BiWidth)
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
            newImage.Сolors[col, row].R = newImage.Сolors[col, row].G = newImage.Сolors[col, row].B = (byte)((Math.Sqrt(Math.Pow((sumX), 2d) + Math.Pow((sumY), 2d))) * 255 / 1000);
        }
    }
}
