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
                var topPlatform = hand.GetTopPlatform(_viewModel.Stage, point.X, point.Y);
                if (hand.CheckCanBuild(_viewModel.Stage, point.X, point.Y, topPlatform.Level))
                {
                    cellBounds = new RectangleF(
                        cellBounds.Left - topPlatform.Level * 0.7f,
                        cellBounds.Top - topPlatform.Level * 0.7f,
                        cellBounds.Width, 
                        cellBounds.Height);

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

        public bool Touch(float x, float y, Rectangle viewportBounds)
        {
            return new Raycast(TestHit).Touch(x, y, viewportBounds)
                || _viewModel.ProcessClickOnBuildableLayer((int)x, (int)y, null);
        }

        private bool TestHit(Point cell, float z0, float z1)
        {
            var hand = _viewModel.Hand as IBuildableItem;
            if (hand == null)
            {
                return false;
            }

            if (!hand.CheckBuildDistance(_viewModel.ActiveEntity.Cell, cell))
            {
                return false;
            }

            var platform = hand.GetTopPlatform(_viewModel.Stage, cell.X, cell.Y);
            if(platform != null && platform.Level <= z1 && platform.Level >= z0)
            {
                _viewModel.ProcessClickOnBuildableLayer(cell.X, cell.Y, platform);
                return true;
            }

            return false;
        }
    }
}