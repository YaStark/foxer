using foxer.Core.Game.Attack;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Linq;

namespace foxer.Core.Game
{
    public class CellWeightAwareProvider : ICellWeightProvider
    {
        private readonly IAwareEntitesProvider _awareEntitesProvider;

        public CellWeightAwareProvider(IAwareEntitesProvider awareEntitesProvider)
        {
            _awareEntitesProvider = awareEntitesProvider;
        }

        public int GetCellWeight(Stage stage, EntityBase entity, int x, int y, IPlatform platform)
        {
            // 1 - асимптотически далеко от всех таргетов, 100 - асимптотически близко
            var entites = _awareEntitesProvider.GetAwareEntites().ToArray();
            if(!entites.Any())
            {
                return 1;
            }

            return (int)(entites.Sum(e => GetEnemyWeight(entity, e)) * 100 / entites.Length);
        }

        private double GetEnemyWeight(EntityBase host, EntityBase enemy)
        {
            var distance = Math.Min(10, Math.Max(1, MathUtils.L1(host.Cell, enemy.Cell)));
            return 1 / distance;
        }
    }
}
