using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Data;

namespace Filters
{
    class Filters
    {
        private Move[] offsets = //метод, "бегающий" по элементам матрицы
        {
            new Move(-2,-2), //сдвиг для матрицы 5*5
            new Move(-1,-1) //3*3
        };

        struct Move
        {
            internal int XOffset;
            internal int YOffset;

            public Move(int x, int y)
            {
                XOffset = x;
                YOffset = y;
            }
        }

        int idim;
        int dim;

        public bool IsAlive = false;
        public int Progress = 0;
        public MyBitmap Image;

        public int iDim
        {
            get
            {
                return idim;
            }
            set
            {
                idim = value;
            }
        }

        public int Dim
        {
            get
            {
                return dim;
            }
            set
            {
                dim = value;
            }
        }

        private void ProgressIncr(AutoResetEvent autoreset)
        {
            Progress++;
            autoreset.Set();
            autoreset.WaitOne();
        }

        public void Filter(int index, string AddressWrite, AutoResetEvent autoreset)
        {
            IsAlive = true;
            MyBitmap newImage = Image;

            switch (index)
            {
                case 1:
                    {
                        iDim = 1;
                        Dim = 3;
                        for (int i = 0; i < Image.Height; i++)
                        {
                            for (int j = 0; j < Image.Width; j++)
                            {
                                Mean(i, j, Image, newImage);
                            }
                            ProgressIncr(autoreset);
                        };
                        break;
                    }

                case 2:
                    {
                        iDim = 0;
                        Dim = 5;
                        for (int i = 0; i < Image.Height; i++)
                        {
                            for (int j = 0; j < Image.Width; j++)
                            {
                                Mean(i, j, Image, newImage);
                            }
                            ProgressIncr(autoreset);
                        };
                        break;
                    }
                case 3:
                    {
                        fil = 1;
                        Sobel(Image, newImage, autoreset);
                        break;
                    }
                case 4:
                    {
                        fil = 2;
                        Sobel(Image, newImage, autoreset);
                        break;
                    }

                case 5:
                    {
                        for (int i = 0; i < Image.Height; i++)
                        {
                            for (int j = 0; j < Image.Width; j++)
                            {
                                GrayScale(i, j, Image, newImage);
                            }
                            ProgressIncr(autoreset);
                        }
                        break;
                    }
                case 6:
                    {
                        for (int i = 0; i < Image.Height; i++)
                        {
                            for (int j = 0; j < Image.Width; j++)
                            {
                                GaussianBlur(i, j, Image, newImage);
                            }
                            ProgressIncr(autoreset);
                        }
                        break;
                    }
            }
            WriteImage(AddressWrite, Image, autoreset);
            IsAlive = false;
            autoreset.Set();
        }

        public MyBitmap ReadImage(string path)
        {
            MyBitmap Image = new MyBitmap();

            FileInfo f = new FileInfo(path);

            BinaryReader br = new BinaryReader(f.OpenRead());

            Image.iType = br.ReadUInt16();
            Image.Size = br.ReadUInt32();
            Image.Reserved1 = br.ReadUInt16();
            Image.Reserved2 = br.ReadUInt16();
            Image.OffBits = br.ReadUInt32();

            Image.ISize = br.ReadUInt32();
            Image.Width = br.ReadInt32();
            Image.Height = br.ReadInt32();
            Image.Planes = br.ReadUInt16();
            Image.BitCount = br.ReadUInt16();
            Image.Compression = br.ReadUInt32();
            Image.SizeImage = br.ReadUInt32();
            Image.XPixForMtr = br.ReadInt32();
            Image.YPixForMtp = br.ReadInt32();
            Image.CtlUsed = br.ReadUInt32();
            Image.ClrImportant = br.ReadUInt32();

            Image.colors = new MyBitmap.tagRGBQUAD[Image.Height, Image.Width];

            for (int i = 0; i < Image.Height; i++)
            {
                for (int j = 0; j < Image.Width; j++)
                {
                    Image.colors[i, j].rgbRed = br.ReadByte();
                    Image.colors[i, j].rgbGreen = br.ReadByte();
                    Image.colors[i, j].rgbBlue = br.ReadByte();
                }
            }

            br.Close();

            return Image;
        }

        private void WriteImage(string path, MyBitmap Image, AutoResetEvent autoreset)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo f2 = new FileInfo(path);
            BinaryWriter bw = new BinaryWriter(f2.OpenWrite());

            bw.Write(Image.iType);
            bw.Write(Image.Size);
            bw.Write(Image.Reserved1);
            bw.Write(Image.Reserved2);
            bw.Write(Image.OffBits);

            bw.Write(Image.ISize);
            bw.Write(Image.Width);
            bw.Write(Image.Height);
            bw.Write(Image.Planes);
            bw.Write(Image.BitCount);
            bw.Write(Image.Compression);
            bw.Write(Image.SizeImage);
            bw.Write(Image.XPixForMtr);
            bw.Write(Image.YPixForMtp);
            bw.Write(Image.CtlUsed);
            bw.Write(Image.ClrImportant);

            for (int i = 0; i < Image.Height; i++)
            {
                for (int j = 0; j < Image.Width; j++)
                {
                    bw.Write(Image.colors[i, j].rgbRed);
                    bw.Write(Image.colors[i, j].rgbGreen);
                    bw.Write(Image.colors[i, j].rgbBlue);
                }
                ProgressIncr(autoreset);
            }
            bw.Close();
        }

        private void Mean(int col, int row, MyBitmap Image, MyBitmap newImage)
        {
            byte count = 0;
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            int y = row + offsets[idim].YOffset;
            int x = col + offsets[idim].XOffset;

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    int newX = x + j;
                    int newY = y + i;
                    if (newX >= 0 && newY >= 0 && newX < Image.Height && newY < Image.Width)
                    {
                        count++;
                        sumR += Image.colors[newX, newY].rgbRed;
                        sumG += Image.colors[newX, newY].rgbGreen;
                        sumB += Image.colors[newX, newY].rgbBlue;
                    }
                }
            }

            newImage.colors[col, row].rgbRed = (byte)(sumR / count);
            newImage.colors[col, row].rgbGreen = (byte)(sumG / count);
            newImage.colors[col, row].rgbBlue = (byte)(sumB / count);
        }

        private void GrayScale(int col, int row, MyBitmap Image, MyBitmap newImage)
        {
            newImage.colors[col, row].rgbRed = newImage.colors[col, row].rgbGreen = newImage.colors[col, row].rgbBlue = (byte)(((byte)Image.colors[col, row].rgbRed + (byte)Image.colors[col, row].rgbGreen + (byte)Image.colors[col, row].rgbBlue) / 3);
        }

        double[,] matGauss = new double[5, 5]
        {
            {0.000789, 0.006581, 0.013347, 0.006581, 0.000789},
            {0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
            {0.013347, 0.111345, 0.225821, 0.111345, 0.013347},
            {0.006581, 0.054901, 0.111345, 0.054901, 0.006581},
            {0.000789, 0.006581, 0.013347, 0.006581, 0.000789},
        };

        private void GaussianBlur(int col, int row, MyBitmap Image, MyBitmap newImage)
        {
            double sR = 0;
            double sG = 0;
            double sB = 0;

            int y = row + offsets[0].YOffset;
            int x = col + offsets[0].XOffset;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int newX = x + j;
                    int newY = y + i;
                    if (newX >= 0 && newY >= 0 && newX < Image.Height && newY < Image.Width)
                    {
                        sR += Image.colors[newX, newY].rgbRed * matGauss[i, j];
                        sG += Image.colors[newX, newY].rgbGreen * matGauss[i, j];
                        sB += Image.colors[newX, newY].rgbBlue * matGauss[i, j];
                    }
                }
            }

            newImage.colors[col, row].rgbRed = (byte)(sR);
            newImage.colors[col, row].rgbGreen = (byte)(sG);
            newImage.colors[col, row].rgbBlue = (byte)(sB);
        }

        int[,] Gx = 
                {
                { -1, -2, -1},
                { 0, 0, 0},
                { 1, 2, 1},
                };

        int[,] Gy = 
                {
                { -1, 0, 1},
                { -2, 0, 2},
                { -1, 0, 1},
                };
        
        private int fil;

        private void Sobel(MyBitmap Image, MyBitmap newImage, AutoResetEvent autoreset)
        {
            int[,] G = new int[3, 3];

            switch (fil)
            {
                case 1:
                    {
                        G = Gx;
                        break;
                    }
                case 2:
                    {
                        G = Gy;
                        break;
                    }

            }

            int[,] BlackWhite = new int[Image.Height, Image.Width];

            int sum = 0;

            for (int i = 0; i < Image.Height; i++)
            {
                for (int j = 0; j < Image.Width; j++)
                {
                    BlackWhite[i, j] = (Image.colors[i, j].rgbRed + Image.colors[i, j].rgbGreen + Image.colors[i, j].rgbBlue) / 3;
                }
                if (i % 2 == 0)
                {
                    ProgressIncr(autoreset);
                }
            }

            for (int col = 0; col < Image.Height; col++)
            {
                for (int row = 0; row < Image.Width; row++)
                {

                    int y = row + offsets[1].YOffset;
                    int x = col + offsets[1].XOffset;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int newX = x + j;
                            int newY = y + i;
                            if (newX >= 0 && newY >= 0 && newX < Image.Height && newY < Image.Width)
                            {
                                sum += BlackWhite[newX, newY] * G[i, j];
                            }
                        }
                    }
                    newImage.colors[col, row].rgbRed = newImage.colors[col, row].rgbGreen = newImage.colors[col, row].rgbBlue = (byte)(Math.Sqrt(sum * sum));
                    sum = 0;
                }
                if (col % 2 == 0)
                {
                    ProgressIncr(autoreset);
                }
            }
        }
    }
}
