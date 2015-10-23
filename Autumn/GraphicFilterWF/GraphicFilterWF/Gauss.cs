
using System.Drawing;

namespace GraphicFilterWF
{
    class Gauss : IFilter
    {
        private static double[,] _gaussMatrix = 
        {
            { 0.000789, 0.006581, 0.013347, 0.006581, 0.000789},
            { 0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
            { 0.013347, 0.111345, 0.225821, 0.111345, 0.013347},
            { 0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
            { 0.000789, 0.006581, 0.013347, 0.006581, 0.000789}
        };

        public Bitmap ApplyFilter(Bitmap image)
        {
            Bitmap newImage = new Bitmap(image);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    ApplyMatrix(i, j, image, newImage);
                    Progress++;
                }
            }
            return newImage;
        }

        public int Progress
        {
            get;
            private set;
        }

        private void ApplyMatrix(int col, int row, Bitmap image, Bitmap newImage)
        {
            double sumR = 0;
            double sumG = 0;
            double sumB = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int X = col + ConvolutionMatrix.GetX5x5(i, j);
                    int Y = row + ConvolutionMatrix.GetY5x5(i, j);

                    if (X >= 0 && Y >= 0 && X < image.Width && Y < image.Height)
                    {
                        sumR += (image.GetPixel(X, Y).R * _gaussMatrix[i, j]);
                        sumG += (image.GetPixel(X, Y).G * _gaussMatrix[i, j]);
                        sumB += (image.GetPixel(X, Y).B * _gaussMatrix[i, j]);
                    }
                }
            }

            newImage.SetPixel(col, row, Color.FromArgb((byte)sumR, (byte)sumG, (byte)sumB));
        }
    }
}
