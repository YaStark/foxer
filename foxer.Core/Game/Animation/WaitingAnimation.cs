using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public class WaitingAnimation : EntityAnimation
    {
        private static readonly Random _rnd = new Random();
        public int TimeMsMin { get; }
        public int TimeMsMax { get; }

        public WaitingAnimation(int timeMsMin, int timeMsMax = 0)
        {
            TimeMsMin = timeMsMin;
            TimeMsMax = timeMsMax == 0 ? timeMsMin : timeMsMax;
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            var time = _rnd.Next(TimeMsMin, TimeMsMax);
            var t = time;
            while(t > 0)
            {
                if(args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                Progress = Math.Min(1, Math.Max(0, 1 - (double)t / time));

                t -= (int)args.DelayMs;
                yield return this;
            }
        }
    }
}
