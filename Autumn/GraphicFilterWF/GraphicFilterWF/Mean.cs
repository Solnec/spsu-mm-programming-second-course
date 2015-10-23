using System;
using System.Drawing;

namespace GraphicFilterWF
{
    class Mean : IFilter
    {
        private readonly int _dim;

        private Func<int, int, int> _getXFunc, _getYFunc;
        
        public Mean(int dim)
        {
            _dim = dim;
            switch (_dim)
            {
                case 3:
                    _getXFunc = ConvolutionMatrix.GetX3x3;
                    _getYFunc = ConvolutionMatrix.GetY3x3;
                    break;
                case 5:
                    _getXFunc = ConvolutionMatrix.GetX5x5;
                    _getYFunc = ConvolutionMatrix.GetY5x5;
                    break;
            }
        }

        public Bitmap ApplyFilter(Bitmap image)
        {
            var newImage = new Bitmap(image);

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
            byte count = 0;
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            for (int i = 0; i < _dim; i++)
            {
                for (int j = 0; j < _dim; j++)
                {
                    int X = col + _getXFunc(i, j);
                    int Y = row + _getYFunc(i, j);

                    if (X >= 0 && Y >= 0 && X < image.Width && Y < image.Height)
                    {
                        count++;
                        sumR += image.GetPixel(X, Y).R;
                        sumG += image.GetPixel(X, Y).G;
                        sumB += image.GetPixel(X, Y).B;
                    }
                }
            }
            newImage.SetPixel(col, row, Color.FromArgb((byte)(sumR / count), (byte)(sumG / count), (byte)(sumB / count)));
        }
    }
}
