using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicFilters;

namespace GraphicFilterWF
{
    class FilterModel
    {
        private BMP _myImage = null;
        private BMP _newImage = null;
        private int _filt;
        private IFilter _filter = null;

        public Filters Filter { get; set; }

        public List<string> FilterList = new List<string>
        {
            "Mean3x3;",
            "Mean5x5;",
            "Greyscale;",
            "Gauss;",
            "SobelX;",
            "SobelY;",
            "Sobel."
        };

        internal enum Filters
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

        public void Load(string path)
        {
            _myImage = new BMP(path);
        }

        public void ChooseFilter(string filter)
        {
            if ((Int32.TryParse(filter, out _filt) && TryChangeFilter((Filters)_filt, out _filter)))
            {

            }
        }

        public void Apply()
        {
            if (_filter != null)
            {
                _newImage = _filter.ApplyFilter(_myImage);
                BMP.BMPInFile(_newImage, _filt.ToString()); 
            }
        }

        public void Save(string path)
        {
            BMP.BMPInFile(_newImage, path); 
        }
    }
}
