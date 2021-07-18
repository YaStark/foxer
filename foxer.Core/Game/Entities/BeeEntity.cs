using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using foxer.Core.Game.Animation;

namespace foxer.Core.Game.Entities
{
    public class BeeEntity : EntityBase, IEscapeStressCellsBehaviorUser
    {
        private readonly float _flySpeedScalarCellPerMs = 0.001f;
        private static readonly Random _rnd = new Random();
        private readonly EscapeStressCellsBehavior _escapeStressCellsBehavior;

        public MovingAnimation Fly { get; }

        public BeeEntity(int x, int y)
             : base(x, y, 2)
        {
            Fly = new MovingAnimation(this, _flySpeedScalarCellPerMs);
            _escapeStressCellsBehavior = new EscapeStressCellsBehavior(this);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if(_escapeStressCellsBehavior.OnUpdate(this, stage, timeMs))
            {
                return;
            }

            if(ActiveAnimation == null)
            {
                var flowers = stage.GetNearestEntitesByType<FlowerEntity>(CellX, CellY, 10).ToArray();
                if (!flowers.Any())
                {
                    var target = new Point(
                        Math.Min(stage.Width - 1, Math.Max(_rnd.Next(-5, 6) + CellX, 0)),
                        Math.Min(stage.Height - 1, Math.Max(_rnd.Next(-5, 6) + CellY, 0)));
                    Fly.Target = target;
                    StartAnimation(Fly.Coroutine);
                }
                else
                {
                    Fly.Target = flowers[_rnd.Next(flowers.Length)].Location;
                    StartAnimation(FlyAndWaitCoroutine);
                }
            }
        }

        private IEnumerable<EntityAnimation> FlyAndWaitCoroutine(EntityCoroutineArgs arg)
        {
            int counter = _rnd.Next(2, 4);
            var target0 = Fly.Target;
            while (counter > 0 && !arg.CancellationToken.IsCancellationRequested)
            {
                foreach (EntityAnimation item in Fly.Coroutine(arg))
                {
                    yield return item;
                }

                Fly.Target = new PointF(
                    target0.X + (float)_rnd.NextDouble() - 0.5f,
                    target0.Y + (float)_rnd.NextDouble() - 0.5f);
                counter--;
            }
        }

        public void BeginEscape(Point[] targets)
        {
            Fly.Target = targets.Last();
            StartAnimation(Fly.Coroutine);
        }
    }
}
