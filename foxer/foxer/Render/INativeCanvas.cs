using System.Drawing;

namespace foxer.Render
{
    public interface INativeCanvas
    {
        SizeF Size { get; }

        void Scale(float x, float y);
        void Translate(float x, float y);
        void RotateDegrees(float angle);
        void DrawImage(byte[] bitmap, RectangleF bounds);
        void DrawRectangle(RectangleF rect, Color color);
        void DrawText(string text, RectangleF rect, Color color);
        void Save();
        void Restore();
    }
}
