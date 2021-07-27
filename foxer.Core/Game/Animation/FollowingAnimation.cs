using foxer.Core.Game.Entities;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public class FollowingAnimation : EntityAnimation
    {
        private readonly EntityBase _parent;
        private readonly MovingAnimation _moving;

        public float Speed { get; }
        public float AnimationSpeed { get; }
        public EntityBase Target { get; set; }

        public FollowingAnimation(EntityBase parent, float speed, float animationSpeed = 0)
        {
            _parent = parent;
            Speed = speed;
            AnimationSpeed = animationSpeed == 0 ? Speed : animationSpeed;
            _moving = new MovingAnimation(parent, speed, animationSpeed);
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            Progress = 0;
            while(true)
            {
                if(args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                bool interrupted = false;
                _moving.Target = Target.Cell;
                foreach (var item in _moving.Coroutine(args))
                {
                    if (Target.PreviousFrameCell != Target.Cell)
                    {
                        interrupted = true;
                        break;
                    }

                    Progress += AnimationSpeed * args.DelayMs;
                    if (Progress > 1)
                    {
                        Progress = 0;
                    }

                    yield return this;
                }

                if (!interrupted)
                {
                    yield break;
                }
            }
        }
    }
}
