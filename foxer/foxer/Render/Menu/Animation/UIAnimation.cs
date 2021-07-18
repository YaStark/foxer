using System.Collections.Generic;

namespace foxer.Render.Menu.Animation
{
    public abstract class UIAnimation
    {
        public double Progress { get; set; }

        public abstract IEnumerable<UIAnimation> Coroutine(UICoroutineArgs args);
    }
}
