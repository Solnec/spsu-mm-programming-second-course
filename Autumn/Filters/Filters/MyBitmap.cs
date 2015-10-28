using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filters
{
    public class MyBitmap
    {
            public ushort iType;
            public uint Size; //Размер файла в байтах.
            public ushort Reserved1;
            public ushort Reserved2; 
            public uint OffBits; //Положение пиксельных данных относительной начала данной структуры (в байтах).

            public uint ISize; //Размер данной структуры в байтах, указывающий так же на версию структуры
            public int Width;
            public int Height;
            public ushort Planes;
            public ushort BitCount; //Содержит количество бит, которое приходится на каждый пиксель
            public uint Compression;
            public uint SizeImage; 
            public int XPixForMtr;
            public int YPixForMtp;
            public uint CtlUsed;
            public uint ClrImportant;
        
        public struct tagRGBQUAD
        {
            public byte rgbRed;
            public byte rgbGreen;
            public byte rgbBlue; 
        }
       
        public tagRGBQUAD[,] colors;
    }
}
