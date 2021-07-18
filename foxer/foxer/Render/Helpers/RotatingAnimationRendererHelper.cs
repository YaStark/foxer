using foxer.Core.Game.Animation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Render.Helpers
{
    public class RotatingAnimationRendererHelper
    {
        private readonly Dictionary<int, byte[][]> _images = new Dictionary<int, byte[][]>();

        public void AddImagesForRotation(int angle, params byte[][] images)
        {
            if (angle < 0) angle = 360 - (-angle % 360);
            else angle = angle % 360;
            _images[angle] = images;
            if (angle == 0)
            {
                _images[360] = images;
            }
        }

        public byte[] GetImageByRotation(int angle, EntityAnimation animation)
        {
            return GetAnimationImage(animation, GetByRotation(_images, angle));
        }

        private static byte[] GetAnimationImage(EntityAnimation animation, byte[][] images)
        {
            int imageIndex = Math.Max(0, Math.Min((int)(animation.Progress * images.Length), images.Length - 1));
            return images[imageIndex];
        }

        private static T GetByRotation<T>(Dictionary<int, T> dict, int angle)
        {
            var key = dict.Keys.OrderBy(x => Math.Abs(x - angle)).First();
            return dict[key];
        }
    }
}
