using foxer.Core.Game.Entities;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Animation
{
    public class MovingByPathAnimation : EntityAnimation
    {
        private readonly MovingAnimation _movingAnimation;
        private readonly EntityBase _parent;

        public float Speed => _movingAnimation.Speed;
        public float AnimationSpeed => _movingAnimation.AnimationSpeed;
        public Point[] Targets { get; set; }

        public MovingByPathAnimation(EntityBase parent, float speed, float animationSpeed = 0)
        {
            _movingAnimation = new MovingAnimation(parent, speed, animationSpeed);
            _parent = parent;
        }

        protected override IEnumerable<EntityAnimation> OnCoroutine(EntityCoroutineArgs args)
        {
            for (int i = 0; i < Targets.Length; i++)
            {
                if(!args.Stage.CheckCanWalkToCell(_movingAnimation.Host, Targets[i]))
                {
                    args.CancellationToken.Cancel();
                    break;
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
