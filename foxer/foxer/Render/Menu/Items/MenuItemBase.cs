using foxer.Core.Utils;
using foxer.Render.Menu.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render.Menu
{
    public abstract class MenuItemBase : IMenuItem
    {
        private static readonly RectangleF[,] _bgSpriteMask = GeomUtils.CreateSpriteMask3x3(0.3f, 0.4f, 0.3f, 0.4f);
        private static readonly byte[] _background = Properties.Resources.background_normal;

        private readonly Coroutine<UIAnimation, UICoroutineArgs> _coroutine = new Coroutine<UIAnimation, UICoroutineArgs>();

        protected UIAnimation ActiveAnimation => _coroutine.Current;

        public void Render(INativeCanvas canvas, MenuItemInfoArgs args)
        {
            OnRender(canvas, args);
        }

        protected virtual void OnRender(INativeCanvas canvas, MenuItemInfoArgs args)
        {
        }

        public virtual void Update(int delayMs)
        {
            _coroutine.Argument.DelayMs = delayMs;
            _coroutine.Update();
        }

        public bool Touch(PointF pt, MenuItemInfoArgs args)
        {
            return OnTouch(pt, args);
        }

        protected virtual bool OnTouch(PointF pt, MenuItemInfoArgs args)
        {
            return false;
        }

        protected void StartAnimation(params Func<UICoroutineArgs, IEnumerable<UIAnimation>>[] coroutines)
        {
            _coroutine.Start(coroutines);
        }

        protected void RenderBackground(INativeCanvas canvas, RectangleF bounds, SizeF cellSize)
        {
            RenderSprite3x3(canvas, _background, _bgSpriteMask, bounds, cellSize);
        }

        protected static void RenderSprite3x3(INativeCanvas canvas, byte[] image, RectangleF[,] spriteMask, RectangleF bounds, SizeF cellSize)
        {
            var size2 = new SizeF(cellSize.Width * 0.5f, cellSize.Height * 0.5f);

            if (bounds.Height > cellSize.Height && bounds.Width > cellSize.Width)
            {
                canvas.DrawImage(image, spriteMask[1, 1], RectangleF.FromLTRB(
                    bounds.Left + size2.Width,
                    bounds.Top + size2.Height,
                    bounds.Right - size2.Height,
                    bounds.Bottom - size2.Height));
            }

            if (bounds.Height > cellSize.Height)
            {
                canvas.DrawImage(image, spriteMask[0, 1], RectangleF.FromLTRB(
                    bounds.Left, 
                    bounds.Top + size2.Height, 
                    bounds.Left + size2.Width, 
                    bounds.Bottom - size2.Height));

                canvas.DrawImage(image, spriteMask[2, 1], RectangleF.FromLTRB(
                    bounds.Right - size2.Width, 
                    bounds.Top + size2.Height, 
                    bounds.Right, 
                    bounds.Bottom - size2.Height));
            }

            if (bounds.Width > cellSize.Width)
            {
                canvas.DrawImage(image, spriteMask[1, 0], RectangleF.FromLTRB(
                    bounds.Left + size2.Width,
                    bounds.Top,
                    bounds.Right - size2.Width,
                    bounds.Top + size2.Height));

                canvas.DrawImage(image, spriteMask[1, 2], RectangleF.FromLTRB(
                    bounds.Left + size2.Width,
                    bounds.Bottom - size2.Height,
                    bounds.Right - size2.Width,
                    bounds.Bottom));
            }

            canvas.DrawImage(image, spriteMask[0, 0],
                new RectangleF(bounds.Left, bounds.Top, size2.Width, size2.Height));
            canvas.DrawImage(image, spriteMask[0, 2],
                new RectangleF(bounds.Left, bounds.Bottom - size2.Height, size2.Width, size2.Height));
            canvas.DrawImage(image, spriteMask[2, 0],
                new RectangleF(bounds.Right - size2.Width, bounds.Top, size2.Width, size2.Height));
            canvas.DrawImage(image, spriteMask[2, 2],
                new RectangleF(bounds.Right - size2.Width, bounds.Bottom - size2.Height, size2.Width, size2.Height));
        }
    }
}
