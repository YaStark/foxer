using foxer.Core.Game.Cells;
using System.Drawing;

namespace foxer.Render
{
    public class RoadRenderer : CellRendererBase<CellRoad>
    {
        private static readonly byte[] _imageRoadLeft;
        private static readonly byte[] _imageRoadRight;
        private static readonly byte[] _imageRoadTop;
        private static readonly byte[] _imageRoadBottom;
        private readonly FloorRenderer _floorRenderer;

        static RoadRenderer()
        {
            _imageRoadLeft = Properties.Resources.road_left;
            _imageRoadTop = Properties.Resources.road_top;
            _imageRoadRight = Properties.Resources.road_right;
            _imageRoadBottom = Properties.Resources.road_bottom;
        }

        public RoadRenderer(FloorRenderer floorRenderer)
        {
            _floorRenderer = floorRenderer;
        }

        protected override void RenderCell(CellRoad cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom)
        {
            _floorRenderer.Render(null, canvas, bounds, left, top, right, bottom);

            if (CanBridge(left)) canvas.DrawImage(_imageRoadLeft, bounds);
            if (CanBridge(top)) canvas.DrawImage(_imageRoadTop, bounds);
            if (CanBridge(right)) canvas.DrawImage(_imageRoadRight, bounds);
            if (CanBridge(bottom)) canvas.DrawImage(_imageRoadBottom, bounds);
        }

        private static bool CanBridge(CellBase cell)
        {
            switch (cell?.Kind)
            {
                case CellKind.Road:
                case CellKind.Door:
                    return true;
            }

            return false;
        }
    }
}
