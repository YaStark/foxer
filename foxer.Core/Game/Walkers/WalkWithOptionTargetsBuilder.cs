using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Core.Game
{
    public class WalkWithOptionTargetsBuilder
    {
        public Point[] ShortestPath { get; }

        public WalkWithOptionTargetsBuilder(
            Stage stage, 
            EntityBase host, 
            ICellWeightProvider weightProvider, 
            ICellAccessibleProvider accessibleProvider,
            params Point[] targets)
        {
            Point[] path = null;
            foreach(var pt in targets)
            {
                var path0 = new WalkBuilder(stage, host, weightProvider, accessibleProvider, pt).Path;
                if (path0 == null) continue;
                else if (path == null) path = path0;
                else if (path.Length > path0.Length) path = path0;
            }

            ShortestPath = path;
        }
    }

    public class WalkToEntityWithOptionTargetsBuilder
    {
        public Point[] ShortestPath { get; }

        public EntityBase NearestEntity { get; }

        public WalkToEntityWithOptionTargetsBuilder(
            Stage stage,
            EntityBase host,
            ICellWeightProvider weightProvider,
            ICellAccessibleProvider accessibleProvider,
            params EntityBase[] targets)
        {
            Point[] path = null;
            EntityBase entity = null;
            foreach (var target in targets)
            {
                var path0 = new WalkBuilder(stage, host, weightProvider, accessibleProvider, target.Cell).Path;
                if (path0 == null) continue;
                else if (path == null || path.Length > path0.Length)
                {
                    path = path0;
                    entity = target;
                }
            }

            ShortestPath = path;
            NearestEntity = entity;
        }
    }
}
