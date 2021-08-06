using foxer.Core.Game.Animation;
using foxer.Core.Game.Attack;
using foxer.Core.Utils;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public class CowEntity : EntityFighterAIBase, IWeaponItem
    {
        public enum CowState
        {
            HeadDown,
            HeadUp
        }

        private const float SPEED_WALK = 0.0009f;
        private const float SPEED_ANIMATION_WALK = SPEED_WALK * 2;

        private const float SPEED_RUN = SPEED_WALK * 10;
        private const float SPEED_ANIMATION_RUN = SPEED_RUN * 2;

        private readonly IAttackerAI _attackerBehavior = new AttackerBehaviorNeutral
        {
            TimeMsToForgive = 5000
        };

        private readonly MoveToTargetAnimation _runToHelp;

        public override IWeaponItem Weapon => this;

        public MovingByPathAnimation Walk { get; }
        public MovingByPathAnimation Run { get; }

        public SimpleAnimation HeadUp { get; }
        public SimpleAnimation HeadDown { get; }

        public EntityAnimation Idle { get; }
        public SimpleAttackAnimation Attack { get; }
        public CowState State { get; private set; } = CowState.HeadUp;

        public CowEntity(int x, int y)
            : base(x, y, 0, 80)
        {
            HeadUp = new SimpleAnimation(200);
            HeadDown = new SimpleAnimation(200);

            Walk = new MovingByPathAnimation(this, SPEED_WALK, SPEED_ANIMATION_WALK);
            Walk.AlwaysRunBefore(
                GameUtils.ConditionalCoroutine(e => State != CowState.HeadDown, HeadDown.Coroutine),
                GameUtils.DelegateCoroutine(e => State = CowState.HeadDown));

            Idle = new WaitingAnimation(500, 5000);
            Idle.AlwaysRunBefore(
                GameUtils.ConditionalCoroutine(e => State != CowState.HeadUp, HeadUp.Coroutine),
                GameUtils.DelegateCoroutine(e => State = CowState.HeadUp));

            Run = new MovingByPathAnimation(this, SPEED_RUN, SPEED_ANIMATION_RUN);

            Attack = new SimpleAttackAnimation(this, Run);

            _runToHelp = new MoveToTargetAnimation(this, Run);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if (_attackerBehavior.OnUpdate(stage, this, timeMs))
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
                    .OrderBy(x => MathUtils.L1(Cell, x.Cell) + stage.Rnd.Next(5))
                    .FirstOrDefault();

                if (target != null)
                {
                    Walk.Targets = randomWalkBuilder.BuildWalkPath(target);
                    StartAnimation(Walk.Coroutine, Idle.Coroutine);
                }
                else
                {
                    StartAnimation(Idle.Coroutine);
                }
            }
        }

        protected override void OnAttacked(Stage stage, EntityFighterBase aggressor)
        {
            base.OnAttacked(stage, aggressor);
            _attackerBehavior.OnAttacked(aggressor);
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
            using (var walker = new RandomWalkBuilder(
                stage,
                new CellWeightAwareProvider(awareEntitesProvider.GetAwareEntites().Select(e => e.Cell)),
                null,
                this,
                10))
            {
                var targets = walker.GetLightestPoints(7, 10, 10);
                if (targets.Any())
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

        public int SwipeMs { get; } = 2000;

        public int HitMs { get; } = 1400;

        public int Distance { get; } = 1;

        public int GetDamage(Stage stage, EntityBase target)
        {
            return stage.Rnd.Next(7, 10);
        }

        public bool CanInteract(EntityFighterBase entity)
        {
            return true;
        }

        #endregion IWeaponItem implementation
    }
}
