using foxer.Core.Game.Animation;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public class WolfEntity : EntityBase
    {
        private const float SPEED_WALK = 0.001f;
        private const float SPEED_ANIMATION_WALK = SPEED_WALK * 2;
        private const double PROB_SIT_AFTER_RND_WALK = 0.5;
        private static readonly Random _rnd = new Random();

        public MovingByPathAnimation Walk { get; }
        public SimpleAnimation Sit { get; }
        public SimpleAnimation Stand { get; }
        public EntityAnimation Idle { get; }
        public EntityAnimation Attack { get; }
        public EntityBodyState State { get; private set; } = EntityBodyState.Stand;

        public WolfEntity(int x, int y)
            : base(x, y, 0)
        {
            Walk = new MovingByPathAnimation(this, SPEED_WALK, SPEED_ANIMATION_WALK);
            Idle = new WaitingAnimation(500, 5000);
            Sit = new SimpleAnimation(250);
            Stand = new SimpleAnimation(200);
            Attack = new SimpleAnimation(800);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if (ActiveAnimation != null)
            {
                return;
            }

            using (var randomWalkBuilder = new RandomWalkBuilder(stage, null, null, this, 5))
            {
                var target = randomWalkBuilder.GetPoints()
                    .OrderBy(x => MathUtils.L1(Cell, x.Cell) + _rnd.Next(5))
                    .FirstOrDefault();

                if (target != null)
                {
                    Walk.Targets = randomWalkBuilder.BuildWalkPath(target);
                    StartAnimation(
                        StandUp,
                        Walk.Coroutine,
                        SitDown,
                        AttackOrIdle);
                }
                else
                {
                    StartAnimation(
                        SitDown,
                        AttackOrIdle);
                }
            }
        }

        private IEnumerable<EntityAnimation> AttackOrIdle(EntityCoroutineArgs arg)
        {
            // todo remove
            if(_rnd.NextDouble() > 0.5)
            {
                foreach(var item in Idle.Coroutine(arg))
                {
                    yield return item;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    foreach (var item in Attack.Coroutine(arg))
                    {
                        yield return item;
                    }
                }
            }
        }

        private IEnumerable<EntityAnimation> StandUp(EntityCoroutineArgs arg)
        {
            if (State == EntityBodyState.Sit)
            {
                foreach (var item in Stand.Coroutine(arg))
                {
                    yield return item;
                }

                State = EntityBodyState.Stand;
            }
        }
        
        private IEnumerable<EntityAnimation> SitDown(EntityCoroutineArgs arg)
        {
            if (State != EntityBodyState.Sit
                 && _rnd.NextDouble() < PROB_SIT_AFTER_RND_WALK
                 && (Rotation < 45 || Rotation > 225))
            {
                State = EntityBodyState.Sit;
                foreach (var item in Sit.Coroutine(arg))
                {
                    yield return item;
                }
            }
        }
    }
}
