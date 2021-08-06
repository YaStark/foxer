using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public class ZMovingAnimation : EntityAnimation
    {
        public EntityBase Host { get; }
        public int DurationMs { get; set; }
        public float DeltaZ { get; }

        public ZMovingAnimation(EntityBase host, int durationMs, float deltaZ)
        {
            Host = host;
            DurationMs = durationMs;
            DeltaZ = deltaZ;
        }

        protected override IEnumerable<EntityAnimation> OnCoroutine(EntityCoroutineArgs args)
        {
            int duration = DurationMs;
            float initialZ = Host.Z;
            while (duration > 0)
            {
                duration -= (int)args.DelayMs;
                Progress = Math.Min(1, Math.Max(0, 1 - (double)duration / DurationMs));
                Host.MoveZ(initialZ + DeltaZ * (1 - (float)Math.Cos(Progress * Math.PI * 2)));
                yield return this;
            }

            Host.MoveZ(initialZ);
            Progress = 0;
        }
    }
}
