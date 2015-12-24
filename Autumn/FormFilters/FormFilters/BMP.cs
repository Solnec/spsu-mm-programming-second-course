using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormFilters;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace GraphicFilters
{
    class BMPFile
    {
        //public int CountOfIteration;

        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
        
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public struct RGB
        {
            public byte R;
            public byte G;
            public byte B;
        }

        public RGB[,] colours;

        public void Read(string adress)
        {
            BinaryReader newFile = null;
            
            FileStream file = new FileStream(adress, FileMode.Open, FileAccess.Read);
            newFile = new BinaryReader(file);

            bfType = newFile.ReadUInt16();
            bfSize = newFile.ReadUInt32();
            bfReserved1 = newFile.ReadUInt16();
            bfReserved2 = newFile.ReadUInt16();
            bfOffBits = newFile.ReadUInt32();

            biSize = newFile.ReadUInt32();
            biWidth = newFile.ReadInt32();
            biHeight = newFile.ReadInt32();
            biPlanes = newFile.ReadUInt16();
            biBitCount = newFile.ReadUInt16();
            biCompression = newFile.ReadUInt32();
            biSizeImage = newFile.ReadUInt32();
            biXPelsPerMeter = newFile.ReadInt32();
            biYPelsPerMeter = newFile.ReadInt32();
            biClrUsed = newFile.ReadUInt32();
            biClrImportant = newFile.ReadUInt32();

            colours = new RGB[biHeight, biWidth];

            for (int i = 0; i < biHeight; i++)
            {
                for (int j = 0; j < biWidth; j++)
                {
                    colours[i, j].B = newFile.ReadByte();
                    colours[i, j].G = newFile.ReadByte();
                    colours[i, j].R = newFile.ReadByte();
                }

                file.Seek((4 - (biBitCount / 8 * biWidth) % 4) % 4, SeekOrigin.Current);
            }

            newFile.Close();
        }

        public void Copy(BMPFile BMP)
        {
            bfType = BMP.bfType;
            bfSize = BMP.bfSize;
            bfReserved1 = BMP.bfReserved1;
            bfReserved2 = BMP.bfReserved2;
            bfOffBits = BMP.bfOffBits;
            
            biSize = BMP.biSize;
            biWidth = BMP.biWidth;
            biHeight = BMP.biHeight;
            biPlanes = BMP.biPlanes;
            biBitCount = BMP.biBitCount;
            biCompression = BMP.biCompression;
            biSizeImage = BMP.biSizeImage;
            biXPelsPerMeter = BMP.biXPelsPerMeter;
            biYPelsPerMeter = BMP.biYPelsPerMeter;
            biClrUsed = BMP.biClrUsed;
            biClrImportant = BMP.biClrImportant;

            colours = BMP.colours;
        }

        public void Write(BMPFile output, string adress)
        {
            BinaryWriter file = null;
            FileStream wfile = new FileStream(adress, FileMode.Create, FileAccess.Write);
            
            file = new BinaryWriter(wfile);

            file.Write(output.bfType);
            file.Write(output.bfSize);
            file.Write(output.bfReserved1);
            file.Write(output.bfReserved2);
            file.Write(output.bfOffBits);

            file.Write(output.biSize);
            file.Write(output.biWidth);
            file.Write(output.biHeight);
            file.Write(output.biPlanes);
            file.Write(output.biBitCount);
            file.Write(output.biCompression);
            file.Write(output.biSizeImage);
            file.Write(output.biXPelsPerMeter);
            file.Write(output.biYPelsPerMeter);
            file.Write(output.biClrUsed);
            file.Write(output.biClrImportant);

            for (int i = 0; i < biHeight; i++)
            {

                for (int j = 0; j < biWidth; j++)
                {
                    file.Write(output.colours[i, j].B);
                    file.Write(output.colours[i, j].G);
                    file.Write(output.colours[i, j].R);
                }

                for (int k = 0; k < (4 - (biBitCount / 8 * biWidth) % 4) % 4; k++)
                    file.Write(true);

            }
            file.Close();
        }
    }
}
