using System;
using System.Drawing;

namespace foxer.Core.Utils
{
    public static class MathUtils
    {
        public static double L1(Point a, Point b)
        {
            return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
        }

        public static double L1(PointF a, PointF b)
        {
            return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
        }

        public static double L1(Point pt)
        {
            return Math.Abs(pt.X) + Math.Abs(pt.Y);
        }

        public static double L2(PointF a, PointF b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
