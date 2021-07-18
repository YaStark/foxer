using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Linq;

namespace foxer.Core.Game
{
    public class EscapeStressCellsBehavior
    {
        private static readonly Random _rnd = new Random();
        private readonly IEscapeStressCellsBehaviorUser _user;

        public bool Escaping { get; private set; }

        public EscapeStressCellsBehavior(IEscapeStressCellsBehaviorUser user)
        {
            _user = user;
        }

        public bool OnUpdate(EntityBase entity, Stage stage, uint timeMs)
        {
            if (Escaping && entity.PreviousFrameCell == entity.Cell)
            {
                return true;
            }

            Escaping = false;
            if (stage.StressManager.GetStressLevelInCell(entity, entity.CellX, entity.CellY) > 0)
            {
                return RunToNearbyCellWithLessStressLevel(entity, stage);
            }

            return false;
        }

        private bool RunToNearbyCellWithLessStressLevel(EntityBase entity, Stage stage)
        {
            var cells = entity.Cell.Nearest4()
                .Where(cell => stage.CheckCanWalkOnCell(entity, cell.X, cell.Y))
                .ToArray();
            if (!cells.Any())
            {
                return false;
            }

            var target = cells.OrderBy(cell => stage.StressManager.GetStressLevelInCell(entity, cell.X, cell.Y)).First();
            Escaping = true;
            _user.BeginEscape(new[] { target });
            return true;
        }
    }
}
