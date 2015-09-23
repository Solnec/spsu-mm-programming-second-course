using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicFilters
{
    class Grayscale : IFilter
    {
        public BMP ApplyFilter(BMP image)
        {
            BMP newImage = new BMP(image);

            for (int i = 0; i < image.BiHeight; i++)
            {
                for (int j = 0; j < image.BiWidth; j++)
                {
                    newImage.Сolors[i, j].R = newImage.Сolors[i, j].G = newImage.Сolors[i, j].B = (byte)((image.Сolors[i, j].B + image.Сolors[i, j].G + image.Сolors[i, j].R) / 3);
                }
            }
            return newImage;
        }
    }
}
