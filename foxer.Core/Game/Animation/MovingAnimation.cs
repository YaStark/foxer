using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Animation
{
    public class MovingAnimation : EntityAnimation
    {
        public EntityBase Host { get; }
        public double Speed { get; }
        public double AnimationSpeed { get; }
        public PointF Target { get; set; }

        public MovingAnimation(EntityBase parent, double speed, double animationSpeed = 0)
        {
            Host = parent;
            Speed = speed;
            AnimationSpeed = animationSpeed == 0 ? Speed : animationSpeed;
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            Progress = 0;
            var walkTarget = Target;
            int signX = Math.Sign(walkTarget.X - Host.X);
            int signY = Math.Sign(walkTarget.Y - Host.Y);
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
                    if (delta > Math.Abs(Host.X - walkTarget.X))
                    {
                        lastOffset.X = (float)(Host.X - walkTarget.X);
                        Host.X = walkTarget.X;
                        readyX = true;
                    }
                    else
                    {
                        lastOffset.X = (float)(signX * delta);
                        Host.X += signX * delta;
                    }

                    Host.CellX = (int)Host.X;
                }

                if (!readyY)
                {
                    if (delta > Math.Abs(Host.Y - walkTarget.Y))
                    {
                        lastOffset.Y = (float)(Host.Y - walkTarget.Y);
                        Host.Y = walkTarget.Y;
                        readyY = true;
                    }
                    else
                    {
                        lastOffset.Y = (float)(signY * delta);
                        Host.Y += signY * delta;
                    }

                    Host.CellY = (int)Host.Y;
                }

                if(!readyX || !readyY)
                {
                    UpdateRotation(Host, walkTarget);
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
