using foxer.Core.ViewModel;
using foxer.Render;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Pages
{
    public class GameLayerCellsRenderer : IGameLayerRenderer
    {
        private static readonly List<ICellRenderer> _cellRenderers = new List<ICellRenderer>();

        private readonly PageGameViewModel _viewModel;

        public bool Enabled { get; } = true;

        static GameLayerCellsRenderer()
        {
            var floorRenderer = new FloorRenderer();
            var roadRenderer = new RoadRenderer(floorRenderer);
            _cellRenderers.Add(floorRenderer);
            _cellRenderers.Add(roadRenderer);
            _cellRenderers.Add(new WaterRenderer());
            _cellRenderers.Add(new BoundRenderer(floorRenderer));
            _cellRenderers.Add(new DoorRenderer(floorRenderer, roadRenderer));
        }

        public GameLayerCellsRenderer(PageGameViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Render(INativeCanvas canvas, IEnumerable<Point> cells)
        {
            foreach(var point in cells.OrderBy(pt => pt.X + pt.Y))
            {
                var cell = _viewModel.Stage.GetCell(point.X, point.Y);
                if (cell == null)
                {
                    continue;
                }

                var cellBounds = new RectangleF(point.X, point.Y, 1, 1);
                cellBounds.Inflate(0.01f, 0.01f);
                    _cellRenderers
                        .FirstOrDefault(r => r.CanRender(cell))
                        ?.Render(
                            cell,
                            canvas,
                            cellBounds,
                            _viewModel.Stage.GetCell(point.X - 1, point.Y),
                            _viewModel.Stage.GetCell(point.X, point.Y - 1),
                            _viewModel.Stage.GetCell(point.X + 1, point.Y),
                            _viewModel.Stage.GetCell(point.X, point.Y + 1));
            }
        }

        public bool Touch(float x, float y, Rectangle viewportBounds)
        {
            return false;
        }
    }
}