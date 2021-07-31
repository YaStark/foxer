using foxer.Core.Game.Animation;
using foxer.Core.Game.Attack;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public class WolfEntity : EntityFighterAIBase, IWeaponItem
    {
        private const float SPEED_WALK = 0.001f;
        private const float SPEED_ANIMATION_WALK = SPEED_WALK * 2;

        private const float SPEED_RUN = SPEED_WALK * 2;
        private const float SPEED_ANIMATION_RUN = SPEED_RUN * 2;

        private const double PROB_SIT_AFTER_RND_WALK = 0.5;
        private static readonly Random _rnd = new Random();

        private readonly IAttackerAI _attackerBehavior = new AttackerBehaviorNeutral
        {
            TimeMsToForgive = 15000
        };

        private readonly MoveToTargetAnimation _runToHelp;

        public override IWeaponItem Weapon => this;

        public MovingByPathAnimation Walk { get; }
        public MovingByPathAnimation Run { get; }

        public SimpleAnimation Sit { get; }
        public SimpleAnimation Stand { get; }
        public EntityAnimation Idle { get; }
        public SimpleAttackAnimation Attack { get; }
        public EntityBodyState State { get; private set; } = EntityBodyState.Stand;

        public WolfEntity(int x, int y)
            : base(x, y, 0, 30)
        {
            Walk = new MovingByPathAnimation(this, SPEED_WALK, SPEED_ANIMATION_WALK);
            Run = new MovingByPathAnimation(this, SPEED_RUN, SPEED_ANIMATION_RUN);

            Idle = new WaitingAnimation(500, 5000);
            Sit = new SimpleAnimation(250);
            Stand = new SimpleAnimation(200);

            Attack = new SimpleAttackAnimation(this, Run);

            _runToHelp = new MoveToTargetAnimation(this, Run);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if(_attackerBehavior.OnUpdate(stage, this, timeMs))
            {
                return;
            }

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
                        Idle.Coroutine);
                }
                else
                {
                    StartAnimation(
                        SitDown,
                        Idle.Coroutine);
                }
            }
        }

        protected override void OnAttacked(Stage stage, EntityFighterBase aggressor)
        {
            base.OnAttacked(stage, aggressor);
            _attackerBehavior.OnAttacked(aggressor);
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

        public override bool BeginFight(Stage stage, EntityFighterBase enemy)
        {
            Attack.Target = enemy;
            StartAnimation(Attack.Coroutine);
            return true;
        }

        public override void EndFight()
        {
            StartAnimation(Idle.Coroutine);
        }

        public override bool BeginRunaway(Stage stage, IAwareEntitesProvider awareEntitesProvider)
        {
            // убегаем к ближайшему другу, если он на расстоянии в 3 клетки и больше
            // иначе просто бежим в сторону от атакующих

            var neighbour = stage.GetEntitesByType(typeof(WolfEntity))
                .Where(w => MathUtils.L1(Cell, w.Cell) > 3)
                .Where(w => MathUtils.L1(Cell, w.Cell) < 10)
                .OrderBy(w => MathUtils.L1(Cell, w.Cell))
                .FirstOrDefault();

            if(neighbour != null)
            {
                _runToHelp.Target = neighbour;
                StartAnimation(_runToHelp.Coroutine);
                return true;
            }

            using(var walker = new RandomWalkBuilder(
                stage, 
                new CellWeightAwareProvider(awareEntitesProvider),
                null,
                this,
                10))
            {
                var targets = walker.GetLightestPoints(7, 10, 10);
                if(targets.Any())
                {
                    var target = targets.OrderBy(pt => MathUtils.L1(pt.Cell, Cell)).LastOrDefault();
                    Run.Targets = walker.BuildWalkPath(target);
                    StartAnimation(Run.Coroutine);
                    return true;
                }
            }

            return false;
        }

        public override bool IsIdle()
        {
            return base.IsIdle()
                || _attackerBehavior.CurrentBehavior == AttackerAIBehavior.Idle;
        }

        #region IWeaponItem implementation

        public WeaponKind WeaponKind { get; } = WeaponKind.Teeth;

        public int SwipeMs { get; } = 1200;

        public int HitMs { get; } = 600;

        public int Distance { get; } = 1;

        public int GetDamage(Stage stage, EntityBase target)
        {
            return stage.Rnd.Next(3, 6);
        }
        
        public bool CanInteract(EntityFighterBase entity)
        {
            return true;
        }

        #endregion IWeaponItem implementation
    }
}
