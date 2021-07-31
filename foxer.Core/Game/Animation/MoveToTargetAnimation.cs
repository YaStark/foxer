using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Animation
{
    public class MoveToTargetAnimation : EntityAnimation
    {
        private Point[] _path;

        public EntityBase Host { get; }

        public MovingByPathAnimation MovingAnimation { get; }

        public EntityBase Target { get; set; }

        public int MinDistance { get; set; }

        public MoveToTargetAnimation(EntityBase host, MovingByPathAnimation movingAnimation, int minDistance = 1)
        {
            Host = host;
            MovingAnimation = movingAnimation;
            MinDistance = minDistance;
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            int maxAttempts = 50;
            while (maxAttempts > 0)
            {
                foreach (var item in MoveToTargetCoroutine(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    yield return item;
                }

                if (MathUtils.L1(Host.Cell, Target.Cell) > MinDistance)
                {
                    // не дошли, перезапускаем построение пути
                    _path = GameUtils.GetPathToItem(args.Stage, Host, Target);
                    if (_path == null)
                    {
                        args.CancellationToken.Cancel();
                        yield break;
                    }
                }
                else
                {
                    // успешный выход
                    Host.RotateTo(Target.Cell);
                    yield break;
                }

                maxAttempts--;
            }

            args.CancellationToken.Cancel();
        }

        private IEnumerable<EntityAnimation> MoveToTargetCoroutine(EntityCoroutineArgs args)
        {
            if (_path == null)
            {
                if (MathUtils.L1(Host.Cell, Target.Cell) > MinDistance)
                {
                    _path = GameUtils.GetPathToItem(args.Stage, Host, Target);
                    if (_path == null)
                    {
                        args.CancellationToken.Cancel();
                        yield break;
                    }
                }
            }

            if (_path != null)
            {
                MovingAnimation.Targets = _path;
                _path = null;
                foreach (var item in MovingAnimation.Coroutine(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    yield return item;

                    if (Target.Cell != Target.PreviousFrameCell)
                    {
                        yield break;
                    }
                }

            }

            Host.RotateTo(Target.Cell);
        }
    }
}
