using foxer.Core.Game.Cells;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render
{
    public class FloorRenderer : CellRendererBase<CellFloor>
    {
        private static readonly byte[,][] _imageGrass;
        private static readonly byte[][] _imageGrassMisc;
        private readonly bool _drawMisc;

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

            if (cell == null) return;
            foreach(var image in EnumerateCellMiscImages(cell.CellFloorKind))
            {
                canvas.DrawImage(image, bounds);
            }
        }

        private static IEnumerable<byte[]> EnumerateCellMiscImages(int kind)
        {
            if ((kind & 1) * (kind & 2) != 0) yield return _imageGrassMisc[0];
            if ((kind & 4) * (kind & 8) != 0) yield return _imageGrassMisc[1];
            if ((kind & 16) * (kind & 32) != 0) yield return _imageGrassMisc[2];
            if ((kind & 64) * (kind & 128) != 0) yield return _imageGrassMisc[3];
        }

        private static bool IsGrassCell(CellBase cell)
        {
            switch(cell?.Kind)
            {
                case null:
                    return false;

                default:
                    return true;
            }
        }
    }
}
