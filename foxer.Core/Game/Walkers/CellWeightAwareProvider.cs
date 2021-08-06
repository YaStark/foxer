using foxer.Core.Game.Attack;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class CellWeightAwareProvider : ICellWeightProvider
    {
        private readonly Point[] _awarePoints;

        public CellWeightAwareProvider(params Point[] points)
        {
            _awarePoints = points;
        }

        public CellWeightAwareProvider(IEnumerable<Point> points)
        {
            _awarePoints = points.ToArray();
        }

        public int GetCellWeight(Stage stage, EntityBase entity, int x, int y, IPlatform platform)
        {
            // 1 - асимптотически далеко от всех таргетов, 100 - асимптотически близко
            if(!_awarePoints.Any())
            {
                return 1;
            }

            return (int)(_awarePoints.Sum(e => GetEnemyWeight(entity, e)) * 100 / _awarePoints.Length);
        }

        private double GetEnemyWeight(EntityBase host, Point pt)
        {
            var distance = Math.Min(10, Math.Max(1, MathUtils.L1(host.Cell, pt)));
            return 1 / distance;
        }
    }
}
