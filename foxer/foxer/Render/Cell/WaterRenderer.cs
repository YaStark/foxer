using foxer.Core.Game.Cells;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render
{
    public class WaterRenderer : CellRendererBase<CellWater>
    {
        private static readonly byte[,][] _imageWater;
        private static readonly byte[][] _imageWaterMisc;

        static WaterRenderer()
        {
            _imageWater = new byte[2, 4][];
            _imageWater[0, 0] = Properties.Resources.water_fill_left;
            _imageWater[0, 1] = Properties.Resources.water_fill_top;
            _imageWater[0, 2] = Properties.Resources.water_fill_right;
            _imageWater[0, 3] = Properties.Resources.water_fill_bottom;

            _imageWater[1, 0] = Properties.Resources.water_corn_left;
            _imageWater[1, 1] = Properties.Resources.water_corn_top;
            _imageWater[1, 2] = Properties.Resources.water_corn_right;
            _imageWater[1, 3] = Properties.Resources.water_corn_bottom;

            _imageWaterMisc = new[]
            {
                Properties.Resources.water_misc_1,
                Properties.Resources.water_misc_2,
            };
        }

        protected override void RenderCell(CellWater cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom)
        {
            canvas.DrawImage(_imageWater[IsWaterCell(left) ? 0 : 1, 0], bounds);
            canvas.DrawImage(_imageWater[IsWaterCell(top) ? 0 : 1, 1], bounds);
            canvas.DrawImage(_imageWater[IsWaterCell(right) ? 0 : 1, 2], bounds);
            canvas.DrawImage(_imageWater[IsWaterCell(bottom) ? 0 : 1, 3], bounds);

            if (cell == null) return;

            RenderCellMisc(cell.Misc, canvas, bounds);
        }

        private static void RenderCellMisc(int kind, INativeCanvas canvas, RectangleF bounds)
        {
            if ((kind & 1) != 0)
            {
                canvas.DrawImage(_imageWaterMisc[0], bounds);
            }

            if ((kind & 2) != 0)
            {
                canvas.DrawImage(_imageWaterMisc[1], bounds);
            }
        }

        private static bool IsWaterCell(CellBase cell)
        {
            return cell != null 
                && cell.Kind == CellKind.Water;
        }
    }
}
