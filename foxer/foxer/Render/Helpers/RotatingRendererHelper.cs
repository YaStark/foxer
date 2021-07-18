using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Render.Helpers
{
    public class RotatingRendererHelper
    {
        private readonly Dictionary<int, byte[]> _images = new Dictionary<int, byte[]>();
        
        public void AddImageForRotation(int angle, byte[] image)
        {
            if (angle < 0) angle = 360 - (-angle % 360);
            else angle = angle % 360;
            _images[angle] = image;
            if (angle == 0)
            {
                _images[360] = image;
            }
        }

        public byte[] GetImageByRotation(int angle)
        {
            return GetByRotation(_images, angle);
        }

        private static T GetByRotation<T>(Dictionary<int, T> dict, int angle)
        {
            var key = dict.Keys.OrderBy(x => Math.Abs(x - angle)).First();
            return dict[key];
        }
    }
}
