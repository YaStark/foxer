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
    public class PlayerEntity : EntityBase
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
        private Point? _newWalkTarget;
        private IPlatform _newWalkTargetPlatform;
        private Point[] _path;
        private int _aggressionResetCounter;

        public IToolItem EmptyHandTool { get; } = new HandTool();

        public ItemBase Hand { get; set; }

        public MovingAnimation Walk { get; }

        public SimpleAnimation Chop { get; }

        public SimpleAnimation Idle { get; }

        public SimpleAnimation ShakeHands { get; }

        public int AggressionLevel { get; private set; }

        public bool WalkMode { get; set; }

        static PlayerEntity()
        {
            _interactors.Add(new PlayerTreeAxeInteractor());
            _interactors.Add(new PlayerHandToolInteractor());
            _interactors.Add(new PlayerDroppedItemInteractor());

            // todo keep it last
            _interactors.Add(new DefaultToolInteractor());
        }

        public PlayerEntity(int x, int y)
            : base(x, y, 0)
        {
            Walk = new MovingAnimation(this, _walkSpeedScalarCellPerMs, _walkSpeedScalarCellPerMs / 1.3f);
            Chop = new SimpleAnimation(2400);
            ShakeHands = new SimpleAnimation(3000);
            Idle = new SimpleAnimation(2000);
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

            if (_newWalkTarget == null)
            {
                if(ActiveAnimation == null)
                {
                    StartAnimation(Idle.Coroutine);
                }

                return;
            }

            var target = _newWalkTarget;
            _newWalkTarget = null;
            if (_newWalkTargetPlatform == null)
            {
                return;
            }

            if (stage.CanBePlaced(this, target.Value.X, target.Value.Y, _newWalkTargetPlatform))
            {
                using (var walker = new WalkBuilder(
                    stage,
                    this,
                    new PlayerWalkWeightProvider(),
                    null,
                    new WalkCell(target.Value, _newWalkTargetPlatform)))
                {
                    var path = walker.GetPath();
                    if (path != null && path.Length < MAX_WALK_DISTANSE)
                    {
                        _path = path;
                        StartAnimation(WalkByPath);
                        return;
                    }
                }
            }

            RotateTo(target.Value);
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

        internal void RotateTo(Point cell)
        {
            Rotation = GeomUtils.GetAngle(cell, Cell);
        }

        public void MoveThenDo(Point[] path, params Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>>[] coroutines)
        {
            _path = path;
            if (_path == null)
            {
                StartAnimation(coroutines);
            }
            else
            {
                StartAnimation(WalkByPath, coroutines);
            }
        }
        
        private IEnumerable<EntityAnimation> WalkByPath(EntityCoroutineArgs arg)
        {
            if (_path == null)
            {
                yield break;
            }

            SetMoveAggression();

            foreach (var pt in _path)
            {
                Walk.Target = pt;
                foreach(var item in Walk.Coroutine(arg))
                {
                    yield return item;
                }
            }

            if(arg.Stage.GetCell(CellX, CellY) is CellDoor door) 
            {
                arg.Stage.LoadLevel(door);
            }
        }

        public void SetWalkTarget(Stage stage, int x, int y, IPlatform platform)
        {
            _newWalkTarget = new Point(x, y);
            _newWalkTargetPlatform = platform;
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
    }
}
