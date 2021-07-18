using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public class SimpleAnimation : EntityAnimation
    {
        public int DurationMs { get; set; }

        public SimpleAnimation(int durationMs)
        {
            DurationMs = durationMs;
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            int duration = DurationMs;
            while (duration > 0)
            {
                if(args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                duration -= (int)args.DelayMs;
                Progress = Math.Min(1, Math.Max(0, 1 - (double)duration / DurationMs));
                yield return this;
            }

            Progress = 0;
        }
    }
}
