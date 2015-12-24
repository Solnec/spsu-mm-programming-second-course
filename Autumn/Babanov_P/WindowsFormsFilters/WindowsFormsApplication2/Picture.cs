using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace WinFormsFilters
{
    public class Picture
    {
        public byte[,] red;
        public byte[,] green;
        public byte[,] blue;
        public byte[] bfType = new byte[2];
        public byte[] bfReserved1 = new byte[2];
        public byte[] bfReserved2 = new byte[2];
        public byte[] NumberOfPlanes = new byte[2];
        public byte[] BitOnPexel = new byte[2];
        public byte[] SizeBitMap = new byte[4];
        public byte[] bfSize = new byte[4];
        public byte[] bfOffBits = new byte[4];
        public byte[] WidthImage = new byte[4];
        public byte[] HeightImage = new byte[4];
        public byte[] TypeOfCompression = new byte[4];
        public byte[] SizeOfCompressionFile = new byte[4];
        public byte[] ResolutionOfWidth = new byte[4];
        public byte[] ResolutionOfHeight = new byte[4];
        public byte[] NumberOfColours = new byte[4];
        public byte[] ImportantColours = new byte[4];
        public int Width, Height;
        public Int32 Progress;
        public Mutex Lock;
        public AutoResetEvent Event;

        public Picture(int width, int height, AutoResetEvent OurEvent)
        {     
            red = new byte[height + 1, width + 1];
            green = new byte[height + 1, width + 1];
            blue = new byte[height + 1, width + 1];
            Width = width;
            Height = height;
            Lock = new Mutex();
            Event = OurEvent;
        }

        public void Mean3()
        {
            int i;
            int j;
            byte[, ]NewRed = new byte[Height + 1, Width + 1];
            byte[, ]NewGreen = new byte[Height + 1, Width + 1];
            byte[, ]NewBlue = new byte[Height + 1, Width + 1];
            for (j = 1; j <= Width; j++)
            {
                NewBlue[1, j] = this.blue[1, j];
                NewGreen[1, j] = this.green[1, j];
                NewRed[1, j] = this.red[1, j];
                NewBlue[Height, j] = this.blue[Height, j];
                NewGreen[Height, j] = this.green[Height, j];
                NewRed[Height, j] = this.red[Height, j];
            }
            Event.Set();
            Event.Set();
            for (i = 2; i <= Height - 1; i++)
            {
                NewBlue[i, 1] = this.blue[i, 1];
                NewGreen[i, 1] = this.green[i, 1];
                NewRed[i, 1] = this.red[i, 1];
                for (j = 2; j <= Width - 1; j++)
                {
                    NewBlue[i, j] = (byte)((this.blue[i, j + 1] + this.blue[i - 1, j] + this.blue[i - 1, j + 1] + this.blue[i - 1, j - 1] + this.blue[i, j - 1] + this.blue[i + 1, j - 1] + this.blue[i + 1, j] + this.blue[i + 1, j + 1]) / 8);
                    NewGreen[i, j] = (byte)((this.green[i, j + 1] + this.green[i - 1, j] + this.green[i - 1, j + 1] + this.green[i - 1, j - 1] + this.green[i, j - 1] + this.green[i + 1, j - 1] + this.green[i + 1, j] + this.green[i + 1, j + 1]) / 8);
                    NewRed[i, j] = (byte)((this.red[i, j + 1] + this.red[i - 1, j] + this.red[i - 1, j + 1] + this.red[i - 1, j - 1] + this.red[i, j - 1] + this.red[i + 1, j - 1] + this.red[i + 1, j] + this.red[i + 1, j + 1]) / 8);
                }
                NewBlue[i, Width] = this.blue[i, Width];
                NewGreen[i, Width] = this.green[i, Width];
                NewRed[i, Width] = this.red[i, Width];
                Event.Set();
            }
            this.green = NewGreen;
            this.red = NewRed;
            this.blue = NewBlue;
        }

        public void Mean5()
        {
            byte[,] NewRed = new byte[Height + 1, Width + 1];
            byte[,] NewGreen = new byte[Height + 1, Width + 1];
            byte[,] NewBlue = new byte[Height + 1, Width + 1];
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    NewBlue[i, j] = this.blue[i, j];
                    NewGreen[i, j] = this.green[i, j];
                    NewRed[i, j] = this.red[i, j];
                    NewBlue[Height - i + 1, j] = this.blue[Height - i + 1, j];
                    NewGreen[Height - i + 1, j] = this.green[Height - i + 1, j];
                    NewRed[Height - i + 1, j] = this.red[Height - i + 1, j];
                }
                Event.Set();
                Event.Set();
                
            }
            for (int i = 3; i <= (Height - 2); i++)
            {
                NewBlue[i, 1] = this.blue[i, 1];
                NewGreen[i, 1] = this.green[i, 1];
                NewRed[i, 1] = this.red[i, 1];
                NewBlue[i, 2] = this.blue[i, 2];
                NewGreen[i, 2] = this.green[i, 2];
                NewRed[i, 2] = this.red[i, 2];
                for (int j = 3; j <= (Width - 2); j++)
                {
                    NewBlue[i, j] = (byte)((this.blue[i, j + 1] + this.blue[i, j + 2] + this.blue[i + 1, j] + this.blue[i + 2, j] + this.blue[i + 1, j + 1] + this.blue[i + 1, j + 2] + this.blue[i + 2, j + 1] + this.blue[i + 2, j + 2] + this.blue[i - 1, j + 2] + this.blue[i - 1, j + 1] + this.blue[i - 1, j + 2] + this.blue[i - 2, j] + this.blue[i - 2, j + 1] + this.blue[i - 2, j + 2] + this.blue[i - 2, j - 1] + this.blue[i - 1, j - 1] + this.blue[i, j - 1] + this.blue[i - 1, j - 1] + this.blue[i - 2, j - 2] + this.blue[i - 1, j - 1] + this.blue[i, j - 1] + this.blue[i - 1, j - 1] + this.blue[i - 2, j - 2]) / 24);
                    NewGreen[i, j] = (byte)((this.green[i, j + 1] + this.green[i, j + 2] + this.green[i + 1, j] + this.green[i + 2, j] + this.green[i + 1, j + 1] + this.green[i + 1, j + 2] + this.green[i + 2, j + 1] + this.green[i + 2, j + 2] + this.green[i - 1, j + 2] + this.green[i - 1, j + 1] + this.green[i - 1, j + 2] + this.green[i - 2, j] + this.green[i - 2, j + 1] + this.green[i - 2, j + 2] + this.green[i - 2, j - 1] + this.green[i - 1, j - 1] + this.green[i, j - 1] + this.green[i - 1, j - 1] + this.green[i - 2, j - 2] + this.green[i - 1, j - 1] + this.green[i, j - 1] + this.green[i - 1, j - 1] + this.green[i - 2, j - 2]) / 24);
                    NewRed[i, j] = (byte)((this.red[i, j + 1] + this.red[i, j + 2] + this.red[i + 1, j] + this.red[i + 2, j] + this.red[i + 1, j + 1] + this.red[i + 1, j + 2] + this.red[i + 2, j + 1] + this.red[i + 2, j + 2] + this.red[i - 1, j + 2] + this.red[i - 1, j + 1] + this.red[i - 1, j + 2] + this.red[i - 2, j] + this.red[i - 2, j + 1] + this.red[i - 2, j + 2] + this.red[i - 2, j - 1] + this.red[i - 1, j - 1] + this.red[i, j - 1] + this.red[i - 1, j - 1] + this.red[i - 2, j - 2] + this.red[i - 1, j - 1] + this.red[i, j - 1] + this.red[i - 1, j - 1] + this.red[i - 2, j - 2]) / 24);
                }
                NewBlue[i, Width - 1] = this.blue[i, Width - 1];
                NewGreen[i, Width - 1] = this.green[i, Width - 1];
                NewRed[i, Width - 1] = this.red[i, Width - 1];
                NewBlue[i, Width] = this.blue[i, Width];
                NewGreen[i, Width] = this.green[i, Width];
                NewRed[i, Width] = this.red[i, Width];
                Event.Set();
            }
            this.green = NewGreen;
            this.red = NewRed;
            this.blue = NewBlue;
        }

        public void Grayscale()
        {
            byte[,] NewRed = new byte[Height + 1, Width + 1];
            byte[,] NewGreen = new byte[Height + 1, Width + 1];
            byte[,] NewBlue = new byte[Height + 1, Width + 1];
            for (int i = 1; i <= (Height); i++)
            {
                for (int j = 1; j <= (Width); j++)
                {
                    NewBlue[i, j] = (byte)((this.blue[i, j] + this.green[i, j] + this.red[i, j]) / 3);
                    NewGreen[i, j] = NewBlue[i, j];
                    NewRed[i, j] = NewGreen[i, j]; 
                }
                Event.Set();                
            }
            this.green = NewGreen;
            this.red = NewRed;
            this.blue = NewBlue;
        }

        public void Gauss()
        {
            byte[,] NewRed = new byte[Height + 1, Width + 1];
            byte[,] NewGreen = new byte[Height + 1, Width + 1];
            byte[,] NewBlue = new byte[Height + 1, Width + 1];
            int i, j;
            for (i = 1; i <= 2; i++)
            {
                for (j = 1; j <= Width; j++)
                {
                    NewBlue[i, j] = this.blue[i, j];
                    NewGreen[i, j] = this.green[i, j];
                    NewRed[i, j] = this.red[i, j];
                    NewBlue[Height - i + 1, j] = this.blue[Height - i + 1, j];
                    NewGreen[Height - i + 1, j] = this.green[Height - i + 1, j];
                    NewRed[Height - i + 1, j] = this.red[Height - i + 1, j];
                    
                }
                Event.Set();
                Event.Set();
            }
            for (i = 3; i <= (Height - 2); i++)
            {
                NewBlue[i, 1] = this.blue[i, 1];
                NewGreen[i, 1] = this.green[i, 1];
                NewRed[i, 1] = this.red[i, 1];
                NewBlue[i, 2] = this.blue[i, 2];
                NewGreen[i, 2] = this.green[i, 2];
                NewRed[i, 2] = this.red[i, 2];
                for (j = 3; j <= (Width - 2); j++)
                {
                    NewBlue[i, j] = (byte)(this.blue[i - 2, j - 2] * 0.000789 + this.blue[i - 2, j - 1] * 0.006581 + this.blue[i - 2, j] * 0.013347 + this.blue[i - 2, j + 1] * 0.006581 + this.blue[i - 2, j + 2] * 0.000789 + this.blue[i - 1, j - 2] * 0.006581 + this.blue[i - 1, j - 1] * 0.054901 + this.blue[i - 1, j] * 0.111345 + this.blue[i - 1, j + 1] * 0.054901 + this.blue[i - 1, j + 2] * 0.006581 + this.blue[i, j - 2] * 0.013347 + this.blue[i, j - 1] * 0.111345 + this.blue[i, j] * 0.225821 + this.blue[i, j + 1] * 0.111345 + this.blue[i, j + 2] * 0.013347 + this.blue[i + 1, j - 2] * 0.006581 + this.blue[i + 1, j - 1] * 0.054901 + this.blue[i + 1, j] * 0.111345 + this.blue[i + 1, j + 1] * 0.054901 + this.blue[i + 1, j + 2] * 0.006581 + this.blue[i + 2, j - 2] * 0.000789 + this.blue[i + 2, j - 1] * 0.006581 + this.blue[i + 2, j] * 0.013347 + this.blue[i + 2, j + 1] * 0.006581 + this.blue[i + 2, j + 2] * 0.000789);
                    NewGreen[i, j] = (byte)(this.green[i - 2, j - 2] * 0.000789 + this.green[i - 2, j - 1] * 0.006581 + this.green[i - 2, j] * 0.013347 + this.green[i - 2, j + 1] * 0.006581 + this.green[i - 2, j + 2] * 0.000789 + this.green[i - 1, j - 2] * 0.006581 + this.green[i - 1, j - 1] * 0.054901 + this.green[i - 1, j] * 0.111345 + this.green[i - 1, j + 1] * 0.054901 + this.green[i - 1, j + 2] * 0.006581 + this.green[i, j - 2] * 0.013347 + this.green[i, j - 1] * 0.111345 + this.green[i, j] * 0.225821 + this.green[i, j + 1] * 0.111345 + this.green[i, j + 2] * 0.013347 + this.green[i + 1, j - 2] * 0.006581 + this.green[i + 1, j - 1] * 0.054901 + this.green[i + 1, j] * 0.111345 + this.green[i + 1, j + 1] * 0.054901 + this.green[i + 1, j + 2] * 0.006581 + this.green[i + 2, j - 2] * 0.000789 + this.green[i + 2, j - 1] * 0.006581 + this.green[i + 2, j] * 0.013347 + this.green[i + 2, j + 1] * 0.006581 + this.green[i + 2, j + 2] * 0.000789);
                    NewRed[i, j] = (byte)(this.red[i - 2, j - 2] * 0.000789 + this.red[i - 2, j - 1] * 0.006581 + this.red[i - 2, j] * 0.013347 + this.red[i - 2, j + 1] * 0.006581 + this.red[i - 2, j + 2] * 0.000789 + this.red[i - 1, j - 2] * 0.006581 + this.red[i - 1, j - 1] * 0.054901 + this.red[i - 1, j] * 0.111345 + this.red[i - 1, j + 1] * 0.054901 + this.red[i - 1, j + 2] * 0.006581 + this.red[i, j - 2] * 0.013347 + this.red[i, j - 1] * 0.111345 + this.red[i, j] * 0.225821 + this.red[i, j + 1] * 0.111345 + this.red[i, j + 2] * 0.013347 + this.red[i + 1, j - 2] * 0.006581 + this.red[i + 1, j - 1] * 0.054901 + this.red[i + 1, j] * 0.111345 + this.red[i + 1, j + 1] * 0.054901 + this.red[i + 1, j + 2] * 0.006581 + this.red[i + 2, j - 2] * 0.000789 + this.red[i + 2, j - 1] * 0.006581 + this.red[i + 2, j] * 0.013347 + this.red[i + 2, j + 1] * 0.006581 + this.red[i + 2, j + 2] * 0.000789);
                }
                NewBlue[i, Width - 1] = this.blue[i, Width - 1];
                NewGreen[i, Width - 1] = this.green[i, Width - 1];
                NewRed[i, Width - 1] = this.red[i, Width - 1];
                NewBlue[i, Width] = this.blue[i, Width];
                NewGreen[i, Width] = this.green[i, Width];
                NewRed[i, Width] = this.red[i, Width];
                Event.Set();
            }
            this.green = NewGreen;
            this.red = NewRed;
            this.blue = NewBlue;
        }

        public void SobelOnX()
        {
            int[,] Intensity = new int[Height + 1, Width + 1];
            int Gx;
            byte[,] NewRed = new byte[Height + 1, Width + 1];
            byte[,] NewGreen = new byte[Height + 1, Width + 1];
            byte[,] NewBlue = new byte[Height + 1, Width + 1];

            for (int i = 1; i <= Height; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    Intensity[i, j] = this.blue[i, j] + this.green[i, j] + this.red[i, j];
                }
            }
            Event.Set();
            for (int i = 2; i < Height; i++)
            {
                for (int j = 2; j < Width; j++)
                {
                    Gx = Math.Abs(-Intensity[i + 1, j - 1] + Intensity[i + 1, j + 1] - 2 * Intensity[i, j - 1] + 2 * Intensity[i, j + 1] - Intensity[i - 1, j - 1] + Intensity[i - 1, j + 1]);
                    NewBlue[i, j] = (byte)(Gx * 255 / 3060);
                    NewGreen[i, j] = NewBlue[i, j];
                    NewRed[i, j] = NewGreen[i, j];

                }
                Event.Set();
            }
            this.green = NewGreen;
            this.red = NewRed;
            this.blue = NewBlue;
        }

        public void SobelOnY()
        {

            int[,] Intensity = new int[Height + 1, Width + 1];
            int Gx;
            byte[,] NewRed = new byte[Height + 1, Width + 1];
            byte[,] NewGreen = new byte[Height + 1, Width + 1];
            byte[,] NewBlue = new byte[Height + 1, Width + 1];

            for (int i = 1; i <= Height; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    Intensity[i, j] = this.blue[i, j] + this.green[i, j] + this.red[i, j];
                }
                Progress = 33 + i * 100/ Height / 3;
            }
            Event.Set();
            for (int i = 2; i < Height; i++)
            {
                for (int j = 2; j < Width; j++)
                {
                    Gx = Math.Abs(-Intensity[i + 1, j - 1] - 2 * Intensity[i + 1, j] - Intensity[i + 1, j + 1] + Intensity[i - 1, j - 1] + 2 * Intensity[i - 1, j] + Intensity[i - 1, j + 1]);
                    NewBlue[i, j] = (byte)(Gx * 255 / 3060);
                    NewGreen[i, j] = NewBlue[i, j];
                    NewRed[i, j] = NewGreen[i, j];
                }
                Event.Set();
            }
            this.green = NewGreen;
            this.red = NewRed;
            this.blue = NewBlue;
        }
        public void Load(string path)
        {
            byte[] width, height;
            width = new byte[4];
            height = new byte[4];
            FileStream f_input = new FileStream(path, FileMode.Open, FileAccess.Read);
            f_input.Seek(18, SeekOrigin.Begin);
            f_input.Read(width, 0, 4);
            f_input.Read(height, 0, 4);
            f_input.Seek(-26, SeekOrigin.Current);
            f_input.Read(this.bfType, 0, 2);
            f_input.Read(this.bfSize, 0, 4);
            f_input.Read(this.bfReserved1, 0, 2);
            f_input.Read(this.bfReserved2, 0, 2);
            f_input.Read(this.bfOffBits, 0, 4);
            f_input.Read(this.SizeBitMap, 0, 4);
            f_input.Read(this.WidthImage, 0, 4);
            f_input.Read(this.HeightImage, 0, 4);
            f_input.Read(this.NumberOfPlanes, 0, 2);
            f_input.Read(this.BitOnPexel, 0, 2);
            f_input.Read(this.TypeOfCompression, 0, 4);
            f_input.Read(this.SizeOfCompressionFile, 0, 4);
            f_input.Read(this.ResolutionOfWidth, 0, 4);
            f_input.Read(this.ResolutionOfHeight, 0, 4);
            f_input.Read(this.NumberOfColours, 0, 4);
            f_input.Read(this.ImportantColours, 0, 4);
            for (int i = 1; i <= Height; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    byte[] tmp = new byte[1];
                    f_input.Read(tmp, 0, 1);
                    this.blue[i, j] = tmp[0];
                    f_input.Read(tmp, 0, 1);
                    this.green[i, j] = tmp[0];
                    f_input.Read(tmp, 0, 1);
                    this.red[i, j] = tmp[0];

                }
                f_input.Seek(Width % 4, SeekOrigin.Current);
            }
            f_input.Close();
       }
        public void Save(int num)
        {
            FileStream f_output = new FileStream("D:\\NewImage" + num.ToString() + ".bmp", FileMode.Create, FileAccess.Write);
            byte [] zero = new byte[3] { 0, 0, 0 };
            f_output.Write(this.bfType, 0, 2);
            f_output.Write(this.bfSize, 0, 4);
            f_output.Write(this.bfReserved1, 0, 2);
            f_output.Write(this.bfReserved2, 0, 2);
            f_output.Write(this.bfOffBits, 0, 4);
            f_output.Write(this.SizeBitMap, 0, 4);
            f_output.Write(this.WidthImage, 0, 4);
            f_output.Write(this.HeightImage, 0, 4);
            f_output.Write(this.NumberOfPlanes, 0, 2);
            f_output.Write(this.BitOnPexel, 0, 2);
            f_output.Write(this.TypeOfCompression, 0, 4);
            f_output.Write(this.SizeOfCompressionFile, 0, 4);
            f_output.Write(this.ResolutionOfWidth, 0, 4);
            f_output.Write(this.ResolutionOfHeight, 0, 4);
            f_output.Write(this.NumberOfColours, 0, 4);
            f_output.Write(this.ImportantColours, 0, 4);
            for (int i = 1; i <= Height; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    byte[] tmp = new byte[1];
                    tmp[0] = this.blue[i, j];
                    f_output.Write(tmp, 0, 1);
                    tmp[0] = this.green[i, j];
                    f_output.Write(tmp, 0, 1);
                    tmp[0] = this.red[i, j];
                    f_output.Write(tmp, 0, 1);
                }
                f_output.Write(zero, 0, Width % 4);
            }
            f_output.Close();
        }
    }
}
