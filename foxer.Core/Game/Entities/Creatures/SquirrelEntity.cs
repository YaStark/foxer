using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using foxer.Core.Game.Animation;
using foxer.Core.Utils;

namespace foxer.Core.Game.Entities
{
    public class SquirrelEntity : EntityBase, IEscapeStressCellsBehaviorUser
    {
        private class WalkCellAccessibleProvider : ICellAccessibleProvider
        {
            public bool CanWalk(Stage stage, EntityBase entity, int x, int y)
            {
                return stage.CheckCanWalkOnCell(entity, x, y)
                    && stage.StressManager.GetStressLevelInCell(entity, x, y) <= 0;
            }
        }

        private const int MAX_SATIETY = 10;
        private static readonly ICellAccessibleProvider _walkCellAccessibleProvider = new WalkCellAccessibleProvider();
        private readonly float _runSpeedScalarCellPerMs = 0.0025f;
        private static readonly Random _rnd = new Random();
        private readonly EscapeStressCellsBehavior _escapeStressCellsBehavior;

        private bool _runAway;
        private bool _hasFood;
        private int _satiety = MAX_SATIETY / 2;

        public MovingByPathAnimation Run { get; }

        public WaitingAnimation Hide { get; }

        public WaitingAnimation Idle { get; }

        public SquirrelEntity(int x, int y)
             : base(x, y, 0)
        {
            Run = new MovingByPathAnimation(this, _runSpeedScalarCellPerMs);
            Hide = new WaitingAnimation(500, 2500);
            Idle = new WaitingAnimation(500, 2500);
            _escapeStressCellsBehavior = new EscapeStressCellsBehavior(this);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if (_runAway)
            {
                RunFromPoint(stage, Cell);
                _runAway = false;
                return;
            }

            if(_escapeStressCellsBehavior.OnUpdate(this, stage, timeMs))
            {
                return;
            }
            
            if (ActiveAnimation == Idle
                && CheckOtherSquirrelsInCell(stage))
            {
                RunFromPoint(stage, Cell);
                return;
            }

            if (ActiveAnimation == null
                && !Feed(stage))
            {
                RunFromPoint(stage, Cell);
            }
        }
        
        public void RunAway()
        {
            _runAway = true;
        }

        private bool CheckOtherSquirrelsInCell(Stage stage)
        {
            return stage.GetEntitesInCell(CellX, CellY)
                        .Any(x => x is SquirrelEntity squirrel
                        && squirrel != this
                        && squirrel.ActiveAnimation == squirrel.Idle);
        }

        private bool Feed(Stage stage)
        {
            // у белки есть еда
            if (_hasFood)
            {
                // попытка найти лужайку чтобы съесть/закопать еду
                return TryFindLawn(stage);
            }

            var trees = stage.GetNearestEntitesByType<TreeEntity>(CellX, CellY, 10)
                .Where(tree => tree.Age > TreeEntity.AGE_MEDIUM)
                .ToArray();

            if(trees.Any())
            {
                // у белки нет еды
                return TryFindTree(stage, trees);
            }

            return false;
        }

        private bool TryFindLawn(Stage stage)
        {
            // идет закапывать желудь или есть его на полянку
            var walkBuilder = new RandomWalkBuilder(stage, null, _walkCellAccessibleProvider, this, 10);
            var lawnCells = walkBuilder.Points
                    .Where(pt => !stage.GetEntitesInCell(pt.X, pt.Y).Any(e => e is TreeEntity))
                    .Where(pt => stage.StressManager.GetStressLevelInCell(this, pt.X, pt.Y) <= 0);
            if (!lawnCells.Any())
            {
                return false;
            }

            Run.Targets = walkBuilder.BuildWalkPath(lawnCells
                .OrderBy(pt => MathUtils.L1(pt, Cell) + _rnd.Next(-5, 6))
                .First());

            if(Run.Targets == null)
            {
                return false;
            }

            StartAnimation(
                Run.Coroutine,
                Idle.Coroutine,
                GameUtils.DelegateCoroutine(FeedOrSeed));

            return true;
        }

        private void FeedOrSeed(EntityCoroutineArgs args)
        {
            if (_rnd.NextDouble() < (double)_satiety / MAX_SATIETY)
            {
                // seed
                args.Stage.TryGrowTree(Cell);
                Starve(false);
            }
            else
            {
                // feed
                _satiety++;
            }

            _hasFood = false;
        }

        private bool TryFindTree(Stage stage, IEnumerable<TreeEntity> trees)
        {
            var targetTrees = trees
                .Where(tree => tree.Guest == null && tree.Cell != Cell)
                .ToArray();

            if(!targetTrees.Any()) return false;

            var builder = new WalkToEntityWithOptionTargetsBuilder(
                stage, this, null,
                _walkCellAccessibleProvider,
                targetTrees);

            Run.Targets = builder.ShortestPath;
            var targetTree = builder.NearestEntity as TreeEntity;
            if (Run.Targets == null || targetTree == null)
            {
                return false;
            }

            targetTree.Guest = this;
            StartAnimation(
                Run.Coroutine,
                HideIfTreeIsThere,
                GameUtils.DelegateCoroutine(args =>
                {
                    _hasFood = CanTakeFood(targetTree);
                    targetTree.Guest = null;
                }));

            return true;
        }

        private IEnumerable<EntityAnimation> HideIfTreeIsThere(EntityCoroutineArgs arg) // todo сделать внешний метод
        {
            if(arg.Stage.GetEntitesInCell(CellX, CellY).Any(e => e is TreeEntity tree && CanHideInTree(tree)))
            {
                foreach (var item in Hide.Coroutine(arg))
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var item in Idle.Coroutine(arg))
                {
                    yield return item;
                }
            }
        }

        private void RunFromPoint(Stage stage, Point awarePoint)
        {
            var walkBuilder = new RandomWalkBuilder(stage, null, _walkCellAccessibleProvider, this, 5);
            var target = walkBuilder
                .Points
                ?.OrderByDescending(pt => MathUtils.L1(pt, awarePoint))
                .FirstOrDefault();

            if (!target.HasValue)
            {
                StartAnimation(Idle.Coroutine);
                return;
            }

            Run.Targets = walkBuilder.BuildWalkPath(target.Value);
            if (Run.Targets != null)
            {
                StartAnimation(Run.Coroutine, Idle.Coroutine);
            }
        }

        private void Starve(bool panic)
        {
            if (panic) _satiety -= MAX_SATIETY / 2;
            else _satiety -= MAX_SATIETY / 5;
        }

        private bool CanHideInTree(TreeEntity tree)
        {
            return tree.Age > TreeEntity.AGE_MEDIUM;
        }

        private bool CanTakeFood(TreeEntity tree)
        {
            return tree.Age > TreeEntity.AGE_LARGE;
        }

        public void BeginEscape(Point[] targets)
        {
            Run.Targets = targets;
            Starve(true);
            StartAnimation(Run.Coroutine);
        }
    }
}
