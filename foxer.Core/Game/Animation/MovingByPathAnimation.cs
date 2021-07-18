using foxer.Core.Game.Entities;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Animation
{
    public class MovingByPathAnimation : EntityAnimation
    {
        private readonly MovingAnimation _movingAnimation;
        private readonly EntityBase _parent;

        public double Speed => _movingAnimation.Speed;
        public double AnimationSpeed => _movingAnimation.AnimationSpeed;
        public Point[] Targets { get; set; }

        public MovingByPathAnimation(EntityBase parent, double speed, double animationSpeed = 0)
        {
            _movingAnimation = new MovingAnimation(parent, speed, animationSpeed);
            _parent = parent;
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            for (int i = 0; i < Targets.Length; i++)
            {
                if(args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                _movingAnimation.Target = Targets[i];
                foreach (var item in _movingAnimation.Coroutine(args))
                {
                    Progress = item.Progress;
                    yield return this;
                }
            }
        }
    }
}
