
using System.Drawing;

namespace GraphicFilterWF
{
    interface IFilter
    {
        Bitmap ApplyFilter(Bitmap image);

        int Progress { get; }
    }
}
