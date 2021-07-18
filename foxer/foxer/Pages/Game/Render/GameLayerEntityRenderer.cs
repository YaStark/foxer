using foxer.Core.Game.Entities;
using foxer.Core.ViewModel;
using foxer.Render;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Pages
{
    public class GameLayerEntityRenderer : IGameLayerRenderer
    {
        private static readonly List<IEntityRenderer> _entityRenderers = new List<IEntityRenderer>();

        private readonly PageGameViewModel _viewModel;

        public bool Enabled { get; } = true;

        static GameLayerEntityRenderer()
        {
            _entityRenderers.Add(new PlayerRenderer());
            _entityRenderers.Add(new FlowerRenderer());
            _entityRenderers.Add(new TreeRenderer());
            _entityRenderers.Add(new BeeRenderer());
            _entityRenderers.Add(new BubblesRenderer());
            _entityRenderers.Add(new WolfRenderer());
            _entityRenderers.Add(new SquirrelRenderer());
            _entityRenderers.Add(new DroppedItemRenderer());
            _entityRenderers.Add(new StoneSmallRenderer());
            _entityRenderers.Add(new StoneBigRenderer());
            _entityRenderers.Add(new StoneOvenRenderer());
        }

        public GameLayerEntityRenderer(PageGameViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Render(INativeCanvas canvas, IEnumerable<Point> cells)
        {
            var entites = cells.SelectMany(cell => _viewModel.Stage.GetEntitesInCell(cell.X, cell.Y));
            foreach (var entity in entites.OrderBy(GetDistanceToCamera))
            {
                var renderer = _entityRenderers.FirstOrDefault(r => r.CanRender(entity));
                if (renderer != null)
                {
                    renderer.Render(
                        canvas,
                        entity,
                        new RectangleF((float)entity.X, (float)entity.Y, 1, 1));
                }
            }
        }

        private double GetDistanceToCamera(EntityBase entity)
        {
            return entity.X + entity.Y + entity.Z;
        }

        public bool Touch(float x, float y)
        {
            return _viewModel.ProcessClickOnEntityLayer(x, y);
        }
    }
}