using foxer.Core.Utils;
using System;
using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public class GrassWallEntity : EntityBase, IWall, IPlatform, IConstruction
    {
        public float Level => Z + GetHeight();

        public WallKind WallKind { get; }

        public ConstructionLevel ConstructionLevel { get; } = ConstructionLevel.Primitive;

        public GrassWallEntity(int x, int y, WallKind wallKind)
            : base(x, y, 0)
        {
            WallKind = wallKind;
        }

        public bool Active(Stage stage)
        {
            return true;
        }

        public bool CanSupport(EntityBase entity)
        {
            if (entity is GrassRoofEntity roof)
            {
                return Rotation == roof.Rotation;
            }

            return false;
        }

        public override float GetHeight() => 1.5f;

        public bool IsColliderFor(Type entityType) => false;
        
        public bool CanTransit(Point nearestTo)
        {
            if (WallKind == WallKind.Door)
            {
                return true;
            }
            
            int side = ((Rotation + 45) % 360) / 90;
            switch (side)
            {
                default:
                case 0: return nearestTo != new Point(CellX + 1, CellY);
                case 1: return nearestTo != new Point(CellX, CellY - 1);
                case 2: return nearestTo != new Point(CellX - 1, CellY);
                case 3: return nearestTo != new Point(CellX, CellY + 1);
            }
        }

        public override float GetZIndex()
        {
            return GameUtils.GetZIndexForWalls(Rotation);
        }
    }
}
