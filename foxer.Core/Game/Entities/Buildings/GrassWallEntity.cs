using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public class GrassWallEntity : EntityBase, IWall
    {
        public GrassWallEntity(int x, int y)
            : base(x, y, 0)
        {
        }

        public bool Active(Stage stage)
        {
            return true;
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
    }
}
