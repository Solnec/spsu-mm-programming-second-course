using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicFilterWF;

namespace GraphicFilterWF
{
    class Grayscale : IFilter
    {
        public Bitmap ApplyFilter(Bitmap image)
        {
            Bitmap newImage = new Bitmap(image);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    byte tmpByte = (byte)((image.GetPixel(i, j).B + image.GetPixel(i, j).G + image.GetPixel(i, j).R) / 3);
                    newImage.SetPixel(i, j, Color.FromArgb(tmpByte, tmpByte, tmpByte));
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
    }
}
