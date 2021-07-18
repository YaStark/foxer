using foxer.Core.Game.Cells;
using System.Drawing;

namespace foxer.Render
{
    public class DoorRenderer : CellRendererBase<CellDoor>
    {
        private readonly FloorRenderer _floorRenderer;
        private readonly RoadRenderer _roadRenderer;

        public DoorRenderer(FloorRenderer floorRenderer, RoadRenderer roadRenderer)
        {
            _floorRenderer = floorRenderer;
            _roadRenderer = roadRenderer;
        }

        protected override void RenderCell(CellDoor cell, INativeCanvas canvas, RectangleF bounds, CellBase left, CellBase top, CellBase right, CellBase bottom)
        {
            _floorRenderer.Render(null, canvas, bounds, left, top, right, bottom);
            _roadRenderer.Render(null, canvas, bounds, left, top, right, bottom);
        }
    }
}
