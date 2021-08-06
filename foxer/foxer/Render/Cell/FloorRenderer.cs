using foxer.Core.Game.Cells;
using System.Drawing;

namespace foxer.Render
{
    public class FloorRenderer : CellRendererBase<CellFloor>
    {
        private static readonly byte[,][] _imageGrass;
        private static readonly byte[][] _imageGrassMisc;

        static FloorRenderer()
        {
            _imageGrass = new byte[2, 4][];
            _imageGrass[0, 0] = Properties.Resources.grass_fill_left;
            _imageGrass[0, 1] = Properties.Resources.grass_fill_top;
            _imageGrass[0, 2] = Properties.Resources.grass_fill_right;
            _imageGrass[0, 3] = Properties.Resources.grass_fill_bottom;

            _imageGrass[1, 0] = Properties.Resources.grass_corn_left;
            _imageGrass[1, 1] = Properties.Resources.grass_corn_top;
            _imageGrass[1, 2] = Properties.Resources.grass_corn_right;
            _imageGrass[1, 3] = Properties.Resources.grass_corn_bottom;

            _imageGrassMisc = new[]
            {
                Properties.Resources.grass_misc_1,
                Properties.Resources.grass_misc_2,
                Properties.Resources.grass_misc_3,
                Properties.Resources.grass_misc_4
            };
        }

        protected override void RenderCell(CellFloor cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom)
        {
            canvas.DrawImage(_imageGrass[IsGrassCell(left) ? 0 : 1, 0], bounds);
            canvas.DrawImage(_imageGrass[IsGrassCell(top) ? 0 : 1, 1], bounds);
            canvas.DrawImage(_imageGrass[IsGrassCell(right) ? 0 : 1, 2], bounds);
            canvas.DrawImage(_imageGrass[IsGrassCell(bottom) ? 0 : 1, 3], bounds);

            if (cell == null)
            {
                return;
            }

            RenderCellMisc(cell.CellFloorKind, canvas, bounds);
        }

        private static void RenderCellMisc(int kind, INativeCanvas canvas, RectangleF bounds)
        {
            if ((kind & 1) * (kind & 2) != 0)
            {
                canvas.DrawImage(_imageGrassMisc[0], bounds);
            }

            if ((kind & 4) * (kind & 8) != 0)
            {
                canvas.DrawImage(_imageGrassMisc[1], bounds);
            }

            if ((kind & 16) * (kind & 32) != 0)
            {
                canvas.DrawImage(_imageGrassMisc[2], bounds);
            }

            if ((kind & 64) * (kind & 128) != 0)
            {
                canvas.DrawImage(_imageGrassMisc[3], bounds);
            }
        }

        private static bool IsGrassCell(CellBase cell)
        {
            return cell != null;
        }
    }
}
