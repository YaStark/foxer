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
        void DrawImage(byte[] bitmap, RectangleF sourceRelative, RectangleF bounds);
        void DrawRectangle(RectangleF rect, Color color);
        void DrawText(string text, RectangleF rect, Color color, HorAlign align);
        void DrawText(string text, RectangleF rect, Color color, float textSize, HorAlign align);
        void Save();
        void Restore();
    }

    public enum HorAlign
    {
        Near,
        Center,
        Far
    }
}
