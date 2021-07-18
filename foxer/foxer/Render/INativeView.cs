using System.Drawing;
using Xamarin.Forms;

namespace foxer.Render
{
    public interface INativeView
    {
        View View { get; }
        SizeF Size { get; }
        void RequestRedraw();
    }
}
