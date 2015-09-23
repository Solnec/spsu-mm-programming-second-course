
namespace GraphicFilters
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

        public BMP ApplyFilter(BMP image)
        {
            BMP newImage = new BMP(image);

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
            double sumR = 0;
            double sumG = 0;
            double sumB = 0;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int X = col + ConvolutionMatrix.GetX5x5(i, j);
                    int Y = row + ConvolutionMatrix.GetY5x5(i, j);

                    if (X >= 0 && Y >= 0 && X < image.BiHeight && Y < image.BiWidth)
                    {
                        sumR += (image.Сolors[X, Y].R * _gaussMatrix[i, j]);
                        sumG += (image.Сolors[X, Y].G * _gaussMatrix[i, j]);
                        sumB += (image.Сolors[X, Y].B * _gaussMatrix[i, j]);
                    }
                }
            }
            newImage.Сolors[col, row].R = (byte)(sumR);
            newImage.Сolors[col, row].G = (byte)(sumG);
            newImage.Сolors[col, row].B = (byte)(sumB);
        }
    }
}
