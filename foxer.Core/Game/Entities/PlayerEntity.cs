using foxer.Core.Game.Animation;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Interactors;
using foxer.Core.Game.Items;
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
            public int GetCellWeight(Stage stage, EntityBase entity, int x, int y)
            {
                float baseWeight = 100;
                float weight = baseWeight;
                if (stage.GetCell(x, y)?.Kind == CellKind.Road)
                {
                    weight = baseWeight / 2;
                }

                weight += stage.GetEntitesInCell(x, y).Count() * baseWeight / 10;
                return (int)weight;
            }
        }

        private const int MAX_WALK_DISTANSE = 15;
        private static readonly List<IInteractor> _interactors = new List<IInteractor>();
        private readonly float _walkSpeedScalarCellPerMs = 0.004f;
        private Point? _newWalkTarget;
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
            _interactors.Add(SimpleToolResourceInteractors.TreeHand);
            _interactors.Add(SimpleToolResourceInteractors.StoneHand);
            _interactors.Add(SimpleToolResourceInteractors.StoneHand);
            _interactors.Add(new PlayerDroppedItemInteractor());
            _interactors.Add(new PlayerGrassInteractor());
        }

        public PlayerEntity(int x, int y)
            : base(x, y)
        {
            Walk = new MovingAnimation(this, _walkSpeedScalarCellPerMs, _walkSpeedScalarCellPerMs / 1.3f);
            Chop = new SimpleAnimation(2400);
            ShakeHands = new SimpleAnimation(3000);
            Idle = new SimpleAnimation(2000);
        }
        
        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            UpdateAggression(timeMs);

            if (!_newWalkTarget.HasValue)
            {
                if(ActiveAnimation == null)
                {
                    StartAnimation(Idle.Coroutine);
                }

                return;
            }

            var pt = _newWalkTarget.Value;
            _newWalkTarget = null;

            if(!WalkMode)
            {
                var arg = new InteractorArgs(stage);
                foreach (var entity in stage.GetEntitesInCell(pt.X, pt.Y))
                {
                    var interactor = _interactors.FirstOrDefault(i => i.CanInteractWith(this, entity, arg));
                    if (interactor != null)
                    {
                        interactor.InteractWith(this, entity, arg);
                        return;
                    }
                }
            }

            if (stage.CheckCanWalkOnCell(this, pt.X, pt.Y))
            {
                var walker = new WalkBuilder(stage, this, new PlayerWalkWeightProvider(), null, pt);
                if (walker.Path != null && walker.Path.Length < MAX_WALK_DISTANSE)
                {
                    _path = walker.Path;
                    StartAnimation(WalkByPath);
                    return;
                }
            }

            RotateTo(pt);
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
            Rotation = (int)(Math.Atan2(CellY - cell.Y, cell.X - CellX) * 180 / Math.PI);
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
                if (!arg.Stage.CheckCanWalkOnCell(this, pt.X, pt.Y))
                {
                    break;
                }

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

        public void SetWalkTarget(Point cell)
        {
            _newWalkTarget = cell;
        }
    }
}
