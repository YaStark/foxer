using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Animation
{
    public class MovingAnimation : EntityAnimation
    {
        public EntityBase Host { get; }
        public float Speed { get; }
        public float AnimationSpeed { get; }
        public PointF Target { get; set; }

        public MovingAnimation(EntityBase parent, float speed, float animationSpeed = 0)
        {
            Host = parent;
            Speed = speed;
            AnimationSpeed = animationSpeed == 0 ? Speed : animationSpeed;
        }

        protected override IEnumerable<EntityAnimation> OnCoroutine(EntityCoroutineArgs args)
        {
            Progress = 0;
            var walkTarget = Target;
            int signX = Math.Sign(walkTarget.X - Host.X);
            int signY = Math.Sign(walkTarget.Y - Host.Y);
            bool readyX = false, readyY = false;
            while (!readyX || !readyY)
            {
                float x = Host.X;
                float y = Host.Y;
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
                        lastOffset.X = Host.X - walkTarget.X;
                        x = walkTarget.X;
                        readyX = true;
                    }
                    else
                    {
                        lastOffset.X = signX * delta;
                        x += signX * delta;
                    }
                }

                if (!readyY)
                {
                    if (delta > Math.Abs(Host.Y - walkTarget.Y))
                    {
                        lastOffset.Y = Host.Y - walkTarget.Y;
                        y = walkTarget.Y;
                        readyY = true;
                    }
                    else
                    {
                        lastOffset.Y = signY * delta;
                        y += signY * delta;
                    }
                }

                if(!readyX || !readyY)
                {
                    if(!Host.TryMoveXY(args.Stage, x, y))
                    {
                        break;
                    }

                    UpdateRotation(Host, walkTarget);
                }

                yield return this;
            }

            Progress = 0;
        }

        private void UpdateRotation(EntityBase entity, PointF target)
        {
            entity.Rotation = ((int)(Math.Atan2(entity.Y - target.Y, target.X - entity.X) / Math.PI * 180) + 720) % 360;
        }
    }
}
