using foxer.Core.Game.Items;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Pages.Game.Menu;
using foxer.Render;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Pages
{
    public class GameLayerBuilderRenderer : IGameLayerRenderer
    {
        private static readonly byte[] _imageCellOk = Properties.Resources.builder_zone_allow;
        private static readonly byte[] _imageCellError = Properties.Resources.builder_zone_denied;

        private readonly GameUIViewModel _viewModel;
        private readonly GameLayerEntityRenderer _entityRenderer;
        private readonly MenuHost _menuHost;

        public GameLayerBuilderRenderer(GameUIViewModel viewModel, GameLayerEntityRenderer entityRenderer, MenuHost menuHost)
        {
            _viewModel = viewModel;
            _entityRenderer = entityRenderer;
            _menuHost = menuHost;
        }

        public bool Enabled => _menuHost.EnsureMenuClosed() && _viewModel.Hand is IBuildableItem;

        public void Render(INativeCanvas canvas, IEnumerable<Point> cells)
        {
            if (!(_viewModel.Hand is IBuildableItem hand))
            {
                return;
            }

            var origin = _viewModel.ActiveEntity.Cell;
            foreach (var point in cells)
            {
                if (!hand.CheckBuildDistance(origin, point))
                {
                    continue;
                }

                var cellBounds = new RectangleF(point.X, point.Y, 1, 1);
                if (hand.CheckCanBuild(_viewModel.Stage, point.X, point.Y))
                {
                    canvas.DrawImage(_imageCellOk, cellBounds);
                    var previewItem = hand.CreatePreviewItem(origin.X, origin.Y, point.X, point.Y);
                    if(previewItem != null)
                    {
                        var bounds = GeomUtils.DeflateTo(cellBounds, new SizeF(0.5f, 0.5f));
                        _entityRenderer.RenderEntity(canvas, bounds, previewItem);
                    }
                }
                else
                {
                    canvas.DrawImage(_imageCellError, cellBounds);
                }
            }
        }

        public bool Touch(float x, float y)
        {
            return _viewModel.ProcessClickOnBuildableLayer(x, y);
        }
    }
}