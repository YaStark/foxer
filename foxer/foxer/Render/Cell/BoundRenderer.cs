using foxer.Core.Game.Cells;
using System.Drawing;

namespace foxer.Render
{
    public class BoundRenderer : CellRendererBase<CellBound>
    {
        private static readonly byte[] _imageBoundsGrass;
        private static readonly byte[] _imageBoundsBridgeRight;
        private static readonly byte[] _imageBoundsBridgeLeft;
        private static readonly byte[] _imageBoundsBridgeTop;
        private static readonly byte[] _imageBoundsBridgeBottom;
        private readonly FloorRenderer _floorRenderer;

        static BoundRenderer()
        {
            _imageBoundsGrass = Properties.Resources.bounds_grass;
            _imageBoundsBridgeBottom = Properties.Resources.bounds_bridge;
            _imageBoundsBridgeRight = Properties.Resources.bounds_bridge_0;
            _imageBoundsBridgeLeft = Properties.Resources.bounds_bridge_1;
            _imageBoundsBridgeTop = Properties.Resources.bounds_bridge_2;
        }

        public BoundRenderer(FloorRenderer floorRenderer)
        {
            _floorRenderer = floorRenderer;
        }

        protected override void RenderCell(CellBound cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom)
        {
            _floorRenderer.Render(null, canvas, bounds, left, top, right, bottom);

            if (CanBridge(left)) canvas.DrawImage(_imageBoundsBridgeLeft, bounds);
            if (CanBridge(top)) canvas.DrawImage(_imageBoundsBridgeTop, bounds);

            canvas.DrawImage(_imageBoundsGrass, bounds);

            if (CanBridge(right)) canvas.DrawImage(_imageBoundsBridgeRight, bounds);
            if (CanBridge(bottom)) canvas.DrawImage(_imageBoundsBridgeBottom, bounds);
        }

        private static bool CanBridge(CellBase cell)
        {
            switch(cell?.Kind)
            {
                case CellKind.Bound:
                    return true;
            }

            return false;
        }
    }
}
