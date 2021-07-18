using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Animation
{
    public class MovingAnimation : EntityAnimation
    {
        private readonly EntityBase _parent;

        public double Speed { get; }
        public double AnimationSpeed { get; }
        public PointF Target { get; set; }

        public MovingAnimation(EntityBase parent, double speed, double animationSpeed = 0)
        {
            _parent = parent;
            Speed = speed;
            AnimationSpeed = animationSpeed == 0 ? Speed : animationSpeed;
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            Progress = 0;
            var walkTarget = Target;
            int signX = Math.Sign(walkTarget.X - _parent.X);
            int signY = Math.Sign(walkTarget.Y - _parent.Y);
            bool readyX = false, readyY = false;
            while (!readyX || !readyY)
            {
                if (args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                var delta = Speed * args.DelayMs;
                Progress += AnimationSpeed * args.DelayMs;
                if (Progress > 1)
                {
                    Progress = 0;
                }

                var lastOffset = new PointF();
                if (!readyX)
                {
                    if (delta > Math.Abs(_parent.X - walkTarget.X))
                    {
                        lastOffset.X = (float)(_parent.X - walkTarget.X);
                        _parent.X = walkTarget.X;
                        readyX = true;
                    }
                    else
                    {
                        lastOffset.X = (float)(signX * delta);
                        _parent.X += signX * delta;
                    }

                    _parent.CellX = (int)_parent.X;
                }

                if (!readyY)
                {
                    if (delta > Math.Abs(_parent.Y - walkTarget.Y))
                    {
                        lastOffset.Y = (float)(_parent.Y - walkTarget.Y);
                        _parent.Y = walkTarget.Y;
                        readyY = true;
                    }
                    else
                    {
                        lastOffset.Y = (float)(signY * delta);
                        _parent.Y += signY * delta;
                    }

                    _parent.CellY = (int)_parent.Y;
                }

                if(!readyX || !readyY)
                {
                    UpdateRotation(_parent, walkTarget);
                }

                yield return this;
            }
        }

        private void UpdateRotation(EntityBase entity, PointF target)
        {
            entity.Rotation = ((int)(Math.Atan2(entity.Y - target.Y, target.X - entity.X) / Math.PI * 180) + 720) % 360;
        }
    }
}
