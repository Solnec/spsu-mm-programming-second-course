using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicFilterWF
{
    public class BMP
    {
        public struct Pixel
        {
            public byte R;
            public byte G;
            public byte B;
        }

        public bool SuccesIn;
        public bool SuccesOut;
        public Pixel[,] Сolors;

        private readonly ushort _bfType;
        private readonly uint _bfSize;
        private readonly ushort _bfReserved1;
        private readonly ushort _bfReserved2;
        private readonly uint _bfOffBits;

        private readonly uint _biSize;
        public readonly int BiWidth;
        public readonly int BiHeight;
        private readonly ushort _biPlanes;
        private readonly ushort _biBitCount;
        private readonly uint _biCompression;
        private readonly uint _biSizeImage;
        private readonly int _biXPelsPerMeter;
        private readonly int _biYPelsPerMeter;
        private readonly uint _biClrUsed;
        private readonly uint _biClrImportant;

        public BMP(string adress)
        {
            BinaryReader changeFile = null;
            try
            {
                ValidateBMP(adress);
                FileStream file = new FileStream(adress, FileMode.Open, FileAccess.Read);
                changeFile = new BinaryReader(file);
                _bfType = changeFile.ReadUInt16();
                _bfSize = changeFile.ReadUInt32();
                _bfReserved1 = changeFile.ReadUInt16();
                _bfReserved2 = changeFile.ReadUInt16();
                _bfOffBits = changeFile.ReadUInt32();

                _biSize = changeFile.ReadUInt32();
                BiWidth = changeFile.ReadInt32();
                BiHeight = changeFile.ReadInt32();
                _biPlanes = changeFile.ReadUInt16();
                _biBitCount = changeFile.ReadUInt16();
                _biCompression = changeFile.ReadUInt32();
                _biSizeImage = changeFile.ReadUInt32();
                _biXPelsPerMeter = changeFile.ReadInt32();
                _biYPelsPerMeter = changeFile.ReadInt32();
                _biClrUsed = changeFile.ReadUInt32();
                _biClrImportant = changeFile.ReadUInt32();

                Сolors = new Pixel[BiHeight, BiWidth];
                for (int i = 0; i < BiHeight; i++)
                {
                    for (int j = 0; j < BiWidth; j++)
                    {
                        Сolors[i, j].B = changeFile.ReadByte();
                        Сolors[i, j].G = changeFile.ReadByte();
                        Сolors[i, j].R = changeFile.ReadByte();
                    }
                    file.Seek(BiWidth % 4, SeekOrigin.Current);
                }
                SuccesIn = true;
            }
            catch (Exception)
            {
                SuccesIn = false;
            }
            finally
            {
                if (changeFile != null)
                {
                    changeFile.Close();
                }
            }


        }

        public BMP(BMP image)
        {
            _bfType = image._bfType;
            _bfSize = image._bfSize;
            _bfReserved1 = image._bfReserved1;
            _bfReserved2 = image._bfReserved2;
            _bfOffBits = image._bfOffBits;

            _biSize = image._biSize;
            BiWidth = image.BiWidth;
            BiHeight = image.BiHeight;
            _biPlanes = image._biPlanes;
            _biBitCount = image._biBitCount;
            _biCompression = image._biCompression;
            _biSizeImage = image._biSizeImage;
            _biXPelsPerMeter = image._biXPelsPerMeter;
            _biYPelsPerMeter = image._biYPelsPerMeter;
            _biClrUsed = image._biClrUsed;
            _biClrImportant = image._biClrImportant;

            Сolors = image.Сolors;
        }

        public static void BMPInFile(BMP image, string adress)
        {
            BinaryWriter changeFile = null;
            try
            {
                ValidateBMP(adress);
                FileStream file = new FileStream(adress, FileMode.Create, FileAccess.Write);
                changeFile = new BinaryWriter(file);
                changeFile.Write(image._bfType);
                changeFile.Write(image._bfSize);
                changeFile.Write(image._bfReserved1);
                changeFile.Write(image._bfReserved2);
                changeFile.Write(image._bfOffBits);

                changeFile.Write(image._biSize);
                changeFile.Write(image.BiWidth);
                changeFile.Write(image.BiHeight);
                changeFile.Write(image._biPlanes);
                changeFile.Write(image._biBitCount);
                changeFile.Write(image._biCompression);
                changeFile.Write(image._biSizeImage);
                changeFile.Write(image._biXPelsPerMeter);
                changeFile.Write(image._biYPelsPerMeter);
                changeFile.Write(image._biClrUsed);
                changeFile.Write(image._biClrImportant);

                for (int i = 0; i < image.BiHeight; i++)
                {
                    for (int j = 0; j < image.BiWidth; j++)
                    {
                        changeFile.Write(image.Сolors[i, j].B);
                        changeFile.Write(image.Сolors[i, j].G);
                        changeFile.Write(image.Сolors[i, j].R);
                    }
                    for (int count = 0; count < image.BiWidth % 4; count++)
                    {
                        changeFile.Write((byte)0);
                    }
                }
                image.SuccesOut = true;
            }
            catch (Exception)
            {
                image.SuccesOut = false;
            }
            finally
            {
                if (changeFile != null) changeFile.Close();
            }
        }
        private static void ValidateBMP(string adress)
        {
            string format = adress.Substring(adress.Length - 4, 4);
            if (!string.Equals(format, ".bmp"))
            {
                throw new IOException();
            }
        }
    }
}
