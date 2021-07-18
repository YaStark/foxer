using foxer.Core.Game.Items;
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
        private readonly MenuHost _menuHost;

        public GameLayerBuilderRenderer(GameUIViewModel viewModel, Game.Menu.MenuHost menuHost)
        {
            _viewModel = viewModel;
            _menuHost = menuHost;
        }

        public bool Enabled => _menuHost.EnsureMenuClosed() && _viewModel.Hand is IBuildableItem;

        public void Render(INativeCanvas canvas, IEnumerable<Point> cells)
        {
            foreach(var point in cells)
            {
                if (!(_viewModel.Hand is IBuildableItem hand)
                    || !hand.CheckBuildDistance(_viewModel.ActiveEntity.Cell, new Point(point.X, point.Y)))
                {
                    continue;
                }

                var cellBounds = new RectangleF(point.X, point.Y, 1, 1);
                if (hand.CheckCanBuild(_viewModel.Stage, point.X, point.Y))
                {
                    canvas.DrawImage(_imageCellOk, cellBounds);
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