using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Render.Helpers
{
    public class RotatingAnimationSpriteRendererHelper : IRendererHelper
    {
        private readonly byte[] _sprite;
        private readonly Dictionary<int, RectangleF[]> _masks = new Dictionary<int, RectangleF[]>();

        public bool Reverse { get; set; } = false;

        public RotatingAnimationSpriteRendererHelper(byte[] sprite, int width, int height)
        {
            _sprite = sprite;
            float x = 1f / width;
            float y = 1f / height;
            for (int i = 0; i < height; i++)
            {
                int angle = i * 360 / height;
                RectangleF[] masksForAngle = new RectangleF[width];
                for (int j = 0; j < width; j++)
                {
                    masksForAngle[j] = new RectangleF(j * x, i * y, x, y);
                }

                _masks[angle] = masksForAngle;
            }
        }
        
        public void RenderImageByRotation(INativeCanvas canvas, RectangleF bounds, int angle, EntityAnimation animation)
        {
            canvas.DrawImage(_sprite, GetByAnimation(animation, GetByRotation(_masks, angle), Reverse), bounds);
        }

        private static T GetByAnimation<T>(EntityAnimation animation, T[] items, bool reverse)
        {
            double progress = animation?.Progress ?? 0;
            int index = Math.Max(0, Math.Min((int)(progress * items.Length), items.Length - 1));
            if(reverse) return items[items.Length - index - 1];
            return items[index];
        }

        private static T GetByRotation<T>(Dictionary<int, T> dict, int angle)
        {
            var key = dict.Keys.OrderBy(x => Math.Abs(x - angle)).First();
            return dict[key];
        }

        public void Render(INativeCanvas canvas, RectangleF bounds, EntityBase entity)
        {
            canvas.DrawImage(
                _sprite, 
                GetByAnimation(entity.ActiveAnimation, GetByRotation(_masks, entity.Rotation), Reverse), 
                bounds);
        }
    }
}
