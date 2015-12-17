using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphicFilterWF
{
    public class FilterModel
    {
        private Bitmap _myImage = null;
        private Bitmap _newImage = null;
        public Action EndOfApply;
        private AutoResetEvent _start = new AutoResetEvent(true);
        private AutoResetEvent _stop = new AutoResetEvent(true);

        private class RefBool
        {
            public RefBool(bool val)
            {
                Value = val;
            }
            public bool Value
            {
                get;
                set;
            }
        }

        private List<RefBool> _boolsApply = new List<RefBool>();
        public Bitmap OldImage()
        {
            return _myImage;
        }

        public Bitmap NewImage()
        {
            return _newImage;
        }

        private IFilter _filter = null;
        public int Size
        {
            get;
            private set;
        }
        public int Progress()
        {
            if (_filter == null)
            {
                TryChangeFilter(Filter, out _filter);
            }
            return _filter.Progress;
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
            _myImage = new Bitmap(InPath);
            Size = _myImage.Height * _myImage.Width;
        }

        private bool _isStart = false;
        private bool _isStop = true;

        public void Start()
        {
            if (_isStart)
            {
                return;
            }
            _isStart = true;
            _boolsApply.Add(new RefBool(true));
            Thread t = new Thread(() => Apply(_boolsApply[_boolsApply.Count - 1]));
            t.IsBackground = true;
            t.Start();
            _isStop = false;
        }

        public void Stop()
        {
            if (_isStop)
            {
                return;
            }
            _isStop = true;
            if (_boolsApply.Count != 0)
            {
                _boolsApply[0].Value = false;
                _boolsApply.RemoveAt(0);
            }
            _isStart = false;
        }

        public void Update()
        {
            TryChangeFilter(Filter, out _filter);
        }
        private void Apply(RefBool isApply)
        {
            if (!TryChangeFilter(Filter, out _filter))
            {
                return;
            }


            if (_filter != null)
            {
                Bitmap oldImage = new Bitmap(_myImage);
                _newImage = _filter.ApplyFilter(oldImage);
            }
            if (EndOfApply != null && isApply.Value)
                EndOfApply();


        }

        public void Save()
        {
            _newImage.Save(OutPath);
        }
    }
}
