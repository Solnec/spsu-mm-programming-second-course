using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicFilterWF
{
    public class FilterModel
    {
        private BMP _myImage = null;
        private BMP _newImage = null;
        private IFilter _filter = null;

        public int Size
        {
            get;
            private set;
        }
        public IProgress Progress
        {
            get;
            private set;
        }

        public string InPath { get; set; }
        public string OutPath { get; set; }

        public Filters Filter { get; set; }

        public List<string> FilterList = new List<string>
        {
            "Mean3x3",
            "Mean5x5",
            "Greyscale",
            "Gauss",
            "SobelX",
            "SobelY",
            "Sobel"
        };

        public enum Filters
        {
            Mean3x3,
            Mean5x5,
            Grayscale,
            Gauss,
            SobelX,
            SobelY,
            Sobel
        };

        static bool TryChangeFilter(Filters filt, out IFilter filter)
        {
            switch (filt)
            {
                case Filters.Mean3x3:
                    filter = new Mean(3);
                    return true;
                case Filters.Mean5x5:
                    filter = new Mean(5);
                    return true;
                case Filters.Grayscale:
                    filter = new Grayscale();
                    return true;
                case Filters.Gauss:
                    filter = new Gauss();
                    return true;
                case Filters.SobelX:
                    filter = new Sobel(0);
                    return true;
                case Filters.SobelY:
                    filter = new Sobel(1);
                    return true;
                case Filters.Sobel:
                    filter = new Sobel(2);
                    return true;
                default:
                    filter = null;
                    return false;
            }
        }

        public void Load()
        {
            _myImage = new BMP(InPath);
            Size = _myImage.BiHeight * _myImage.BiWidth;
        }


        public void Apply()
        {
            if (!TryChangeFilter(Filter, out _filter))
                return;
            Progress = (IProgress)_filter;
            if (_filter != null)
            {
                _newImage = _filter.ApplyFilter(_myImage);
                BMP.BMPInFile(_newImage, Filter.ToString());
            }
        }

        public void Save()
        {
            BMP.BMPInFile(_newImage, OutPath);
        }
    }
}
