using System;
using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public class GrassWallEntity : EntityBase, IWall, IPlatform
    {
        public float Level => Z + GetHeight();

        public GrassWallEntity(int x, int y, float z)
            : base(x, y, z)
        {
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

        public override float GetHeight()
        {
            return 1.8f;
        }

        public Point GetTransitPreventionTarget()
        {
            int side = ((Rotation + 45) % 360) / 90;
            switch (side)
            {
                default:
                case 0: return new Point(CellX + 1, CellY);
                case 1: return new Point(CellX, CellY - 1);
                case 2: return new Point(CellX - 1, CellY);
                case 3: return new Point(CellX, CellY + 1);
            }
        }

        public bool IsColliderFor(Type entityType)
        {
            return false;
        }
    }
}
