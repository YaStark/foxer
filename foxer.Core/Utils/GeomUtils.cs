using System;
using System.Drawing;

namespace foxer.Core.Utils
{
    public static class GeomUtils
    {
        public static RectangleF DeflateTo(RectangleF rect, SizeF size)
        {
            float dx = (rect.Width - size.Width) / 2;
            float dy = (rect.Height - size.Height) / 2;
            return Deflate(rect, dx, dy);
        }

        public static RectangleF Deflate(RectangleF rect, float left, float top, float right, float bottom)
        {
            return RectangleF.FromLTRB(rect.Left + left, rect.Top + top, rect.Right - right, rect.Bottom - bottom);
        }

        public static RectangleF Deflate(RectangleF rect, float dx, float dy)
        {
            return Deflate(rect, dx, dy, dx, dy);
        }

        public static int GetAngle(Point a, Point b)
        {
            return ((int)(Math.Atan2(b.Y - a.Y, a.X - b.X) * 180 / Math.PI) + 360) % 360;
        }

        public static int GetAngle90(Point a, Point b)
        {
            return (((int)Math.Round(Math.Atan2(b.Y - a.Y, a.X - b.X) * 2 / Math.PI) + 4) % 4) * 90;
        }

        public static RectangleF Deflate(RectangleF rect, float delta)
        {
            return Deflate(rect, delta, delta);
        }


        public static SizeF Deflate(SizeF size, float dx, float dy)
        {
            return new SizeF(size.Width - dx, size.Height - dy);
        }

        public static SizeF Deflate(SizeF size, float delta)
        {
            return Deflate(size, delta, delta);
        }

        public static RectangleF[,] SplitOnCells(RectangleF bounds, int width, int height, float relMargin = 0.05f)
        {
            var result = new RectangleF[width, height];
            float cellSizeW = (bounds.Width / width - relMargin) / (1 + relMargin); 
            float cellSizeH = (bounds.Height / height - relMargin) / (1 + relMargin);
            int cellSize = (int)Math.Min(cellSizeW, cellSizeH);
            float gapX = width > 1 ? (bounds.Width - cellSize * width) / (width - 1) : 0;
            float gapY = height > 1 ? (bounds.Height - cellSize * height) / (height - 1) : 0;
            Size cellSizeX = new Size(cellSize, cellSize);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i, j] = new RectangleF(
                        bounds.X + i * (cellSize + gapX), 
                        bounds.Y + j * (cellSize + gapY), 
                        cellSize, cellSize);
                }
            }

            return result;
        }

        public static RectangleF[,] SplitOnCells(SizeF size, int width, int height, float relMargin = 0.05f)
        {
            return SplitOnCells(new RectangleF(PointF.Empty, size), width, height, relMargin);
        }

        public static RectangleF[,] CreateSpriteMask3x3(float x1, float x2, float y1, float y2)
        {
            var x3 = 1 - x1 - x2;
            var y3 = 1 - y1 - y2;
            var result = new RectangleF[3, 3];
            result[0, 0] = new RectangleF(0, 0, x1, y1);
            result[1, 0] = new RectangleF(x1, 0, x2, y1);
            result[2, 0] = new RectangleF(x1 + x2, 0, x3, y1);

            result[0, 1] = new RectangleF(0, y1, x1, y2);
            result[1, 1] = new RectangleF(x1, y1, x2, y2);
            result[2, 1] = new RectangleF(x1 + x2, y1, x3, y2);

            result[0, 2] = new RectangleF(0, y1 + y2, x1, y3);
            result[1, 2] = new RectangleF(x1, y1 + y2, x2, y3);
            result[2, 2] = new RectangleF(x1 + x2, y1 + y2, x3, y3);

            return result;
        }

        public static RectangleF[,] CreateUniformSpriteMask(int width, int height)
        {
            var result = new RectangleF[width, height];

            float x = 1f / width;
            float y = 1f / height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[i, j] = new RectangleF(i * x, j * y, x, y);
                }
            }

            return result;
        }
    }
}
