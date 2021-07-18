using foxer.Core.Game.Animation;
using System;

namespace foxer.Render.Helpers
{
    public class AnimationRendererHelper
    {
        private readonly byte[][] _images;

        public AnimationRendererHelper(params byte[][] images)
        {
            _images = images;
        }

        public byte[] GetImage(EntityAnimation animation)
        {
            return GetAnimationImage(animation, _images);
        }

        private static byte[] GetAnimationImage(EntityAnimation animation, byte[][] images)
        {
            int imageIndex = Math.Max(0, Math.Min((int)(animation.Progress * images.Length), images.Length - 1));
            return images[imageIndex];
        }
    }
}
