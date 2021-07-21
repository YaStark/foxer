using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Render.Helpers
{
    public class RotatingSpriteRendererHelper
    {
        private readonly Dictionary<int, RectangleF> _masks = new Dictionary<int, RectangleF>();
        private readonly byte[] _image;

        public RotatingSpriteRendererHelper(byte[] image, int height)
        {
            _image = image;
            float y = 1f / height;
            for (int i = 0; i < height; i++)
            {
                int angle = i * 360 / height;
                _masks[angle] = new RectangleF(0, i * y, 1, y);
            }
        }

        public void Render(INativeCanvas canvas, RectangleF bounds, int angle)
        {
            canvas.DrawImage(_image, GetByRotation(_masks, angle), bounds);
        }

        private static T GetByRotation<T>(Dictionary<int, T> dict, int angle)
        {
            var key = dict.Keys.OrderBy(x => Math.Abs(x - angle)).First();
            return dict[key];
        }
    }
}
