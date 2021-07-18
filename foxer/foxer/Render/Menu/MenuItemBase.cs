using foxer.Core.Utils;
using foxer.Render.Menu.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render.Menu
{
    public abstract class MenuItemBase : IMenuItem
    {
        private readonly Coroutine<UIAnimation, UICoroutineArgs> _coroutine = new Coroutine<UIAnimation, UICoroutineArgs>();

        protected UIAnimation ActiveAnimation => _coroutine.Current;

        public void Render(INativeCanvas canvas, RectangleF bounds)
        {
            OnRender(canvas, bounds);
        }

        protected virtual void OnRender(INativeCanvas canvas, RectangleF bounds)
        {
        }

        public virtual void Update(int delayMs)
        {
            _coroutine.Argument.DelayMs = delayMs;
            _coroutine.Update();
        }

        public bool Touch(PointF pt, RectangleF bounds)
        {
            return OnTouch(pt, bounds);
        }

        protected virtual bool OnTouch(PointF pt, RectangleF bounds)
        {
            return false;
        }

        protected void StartAnimation(params Func<UICoroutineArgs, IEnumerable<UIAnimation>>[] coroutines)
        {
            _coroutine.Start(coroutines);
        }

    }
}
