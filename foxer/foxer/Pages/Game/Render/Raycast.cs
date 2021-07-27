using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Pages
{
    public class Raycast
    {
        private readonly Func<Point, float, float, bool> _testHit;

        public Raycast(Func<Point, float, float, bool> testHit)
        {
            _testHit = testHit;
        }

        public bool Touch(float x, float y, Rectangle viewportBounds)
        {
            int cellX = (int)x;
            int cellY = (int)y;

            // t = [0..inf]
            // xs = t + x
            // ys = t + y
            // zs = 0.7f * t

            // xs = cellX + 1, tx = cellx + 1 - x
            // ys = cellY + 1, ty = celly + 1 - y

            List<(Point, float, float)> hits = new List<(Point, float, float)>();

            float xs = x;
            float ys = y;
            float z = 0;
            while(cellX <= viewportBounds.Right || cellY <= viewportBounds.Bottom)
            {
                if (cellX - x > cellY - y)
                {
                    var z0 = (cellY - y + 1) / 0.7f;
                    hits.Add((new Point(cellX, cellY), z, z0));
                    cellY++;
                    z = z0;
                }
                else
                {
                    var z0 = (cellX - x + 1) / 0.7f;
                    hits.Add((new Point(cellX, cellY), z, z0));
                    cellX++;
                    z = z0;
                }
            }

            hits.Reverse();
            foreach(var item in hits)
            {
                if(TestHit(item.Item1, item.Item2, item.Item3))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TestHit(Point cell, float z0, float z1)
        {
            return _testHit.Invoke(cell, z0, z1);
        }
    }
}