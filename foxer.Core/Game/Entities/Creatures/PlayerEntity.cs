using foxer.Core.Game.Animation;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Interactors;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public class PlayerEntity : EntityBase, ISingletoneFactory<IWeaponItem>
    {
        private class PlayerWalkWeightProvider : ICellWeightProvider
        {
            public int GetCellWeight(Stage stage, EntityBase entity, int x, int y, IPlatform platform)
            {
                float baseWeight = 100;
                float weight = baseWeight;
                weight += stage.GetOverlappedEntites(entity, x, y, platform.Level).Count() * baseWeight / 10;
                return (int)weight;
            }
        }

        private const int MAX_WALK_DISTANSE = 15;
        private static readonly List<IInteractor> _interactors = new List<IInteractor>();
        private readonly float _walkSpeedScalarCellPerMs = 0.004f;
        private int _aggressionResetCounter;

        public override IAttacker Attacker { get; }

        public IToolItem EmptyHandTool { get; } = new HandTool();

        public ItemBase Hand { get; set; }

        public MovingByPathAnimation Walk { get; }

        public SimpleAnimation Idle { get; }

        public SimpleAnimation ShakeHands { get; }

        public PlayerGatherAnimation ToolWork { get; }

        public MoveToTargetAnimation MoveToTarget { get; }

        public SimpleAttackAnimation Attack { get; }

        public int AggressionLevel { get; private set; }

        public bool WalkMode { get; set; }

        IWeaponItem ISingletoneFactory<IWeaponItem>.Item => Hand as IWeaponItem;

        static PlayerEntity()
        {
            _interactors.Add(new PlayerAttackInteractor());
            _interactors.Add(new PlayerResourceInteractor());
            _interactors.Add(new PlayerDroppedItemInteractor());
        }

        public PlayerEntity(int x, int y)
            : base(x, y, 0)
        {
            Walk = new MovingByPathAnimation(this, _walkSpeedScalarCellPerMs, _walkSpeedScalarCellPerMs / 1.3f);
            ToolWork = new PlayerGatherAnimation(this);
            MoveToTarget = new MoveToTargetAnimation(this, Walk);
            Attack = new SimpleAttackAnimation(this, Walk);
            ShakeHands = new SimpleAnimation(3000);
            Idle = new SimpleAnimation(2000);

            Attacker = new SimpleAttacker(Attack, this);
        }

        public override bool UseHitbox()
        {
            return false;
        }

        public override float GetHeight()
        {
            return 1.45f;
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            UpdateAggression(timeMs);
            if (ActiveAnimation == null)
            {
                StartAnimation(Idle.Coroutine);
            }
        }

        private void UpdateAggression(uint timeMs)
        {
            if (AggressionLevel <= 0)
            {
                return;
            }

            _aggressionResetCounter += (int)timeMs;
            if(_aggressionResetCounter > 1000)
            {
                AggressionLevel--;
                _aggressionResetCounter = 0;
            }
        }

        private void SetAggression(int level)
        {
            AggressionLevel = Math.Max(level, AggressionLevel);
            _aggressionResetCounter = 0;
        }

        public void SetMoveAggression()
        {
            SetAggression(5);
        }

        public void SetWorkAggression()
        {
            SetAggression(8);
        }

        private void LoadLevel(EntityCoroutineArgs arg)
        {
            if(arg.Stage.GetCell(CellX, CellY) is CellDoor door) 
            {
                arg.Stage.LoadLevel(door);
            }
        }

        public void SetWalkTarget(Stage stage, int x, int y, IPlatform platform)
        {
            if (!stage.CanBePlaced(this, x, y, platform))
            {
                return;
            }

            var pt = new Point(x, y);
            using (var walker = new WalkBuilder(
                stage,
                this,
                new PlayerWalkWeightProvider(),
                null,
                new WalkCell(pt, platform)))
            {
                var path = walker.GetPath();
                if (path != null && path.Length < MAX_WALK_DISTANSE)
                {
                    Walk.Targets = path;
                    StartAnimation(
                        GameUtils.DelegateCoroutine(e => SetMoveAggression()),
                        Walk.Coroutine,
                        GameUtils.DelegateCoroutine(LoadLevel));
                }
                else
                {
                    RotateTo(pt);
                }
            }
        }

        public bool TryInteract(Stage stage, EntityBase entity)
        {
            if (WalkMode)
            {
                return false;
            }

            var arg = new InteractorArgs(stage);
            var interactor = _interactors.FirstOrDefault(i => i.CanInteractWith(this, entity, arg)); // todo check Z in interactors
            if (interactor != null)
            {
                return interactor.InteractWith(this, entity, arg);
            }

            return false;
        }

        public bool TryGather(Stage stage, EntityBase target)
        {
            if(target != null)
            {
                ToolWork.Target = target;
                StartAnimation(ToolWork.Coroutine);
                return true;
            }

            return false;
        }

        public bool TryAttack(Stage stage, EntityBase target)
        {
            if (target != null)
            {
                Attack.Target = target;
                StartAnimation(Attack.Coroutine);
                return true;
            }

            return false;
        }

        public bool TryRunToTarget(
            Stage stage, 
            EntityBase target, 
            int minDistance,
            params Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>>[] coroutines)
        {
            if (target != null)
            {
                MoveToTarget.Target = target;
                StartAnimation(MoveToTarget.Coroutine, coroutines);
                return true;
            }

            return false;
        }
    }
}
