using System;

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

        public BMP ApplyFilter(BMP image)
        {
            var newImage = new BMP(image);

            for (int i = 0; i < image.BiHeight; i++)
            {
                for (int j = 0; j < image.BiWidth; j++)
                {
                    ApplyMatrix(i, j, image, newImage);
                }
            }
            return newImage;
        }

        private void ApplyMatrix(int col, int row, BMP image, BMP newImage)
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

                    if (X >= 0 && Y >= 0 && X < image.BiHeight && Y < image.BiWidth)
                    {
                        count++;
                        sumR += image.Сolors[X, Y].R;
                        sumG += image.Сolors[X, Y].G;
                        sumB += image.Сolors[X, Y].B;
                    }
                }
            }
            newImage.Сolors[col, row].R = (byte)(sumR / count);
            newImage.Сolors[col, row].G = (byte)(sumG / count);
            newImage.Сolors[col, row].B = (byte)(sumB / count);
        }
    }
}
