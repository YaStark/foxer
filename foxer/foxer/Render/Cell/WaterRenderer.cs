﻿using foxer.Core.Game.Cells;
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
            foreach (var image in EnumerateCellMiscImages(cell.Misc))
            {
                canvas.DrawImage(image, bounds);
            }
        }

        private static IEnumerable<byte[]> EnumerateCellMiscImages(int kind)
        {
            if ((kind & 1) != 0) yield return _imageWaterMisc[0];
            if ((kind & 2) != 0) yield return _imageWaterMisc[1];
        }

        private static bool IsWaterCell(CellBase cell)
        {
            switch (cell?.Kind)
            {
                case CellKind.Water:
                    return true;

                default:
                    return false;
            }
        }
    }
}
