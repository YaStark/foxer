using foxer.Core.Game.Entities;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public abstract class EntityAnimation
    {
        public double Progress { get; set; }

        public abstract IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args);
    }
}
