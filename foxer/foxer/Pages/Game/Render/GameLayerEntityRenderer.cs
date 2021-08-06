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
            _entityRenderers.Add(new CowRenderer());
            _entityRenderers.Add(new SquirrelRenderer());
            _entityRenderers.Add(new DroppedItemRenderer());
            _entityRenderers.Add(new StoneSmallRenderer());
            _entityRenderers.Add(new StoneBigRenderer());
            _entityRenderers.Add(new StoneOvenRenderer());
            _entityRenderers.Add(new GrassRenderer());
            _entityRenderers.Add(new GrassWallRenderer());
            _entityRenderers.Add(new GrassFloorRenderer());
            _entityRenderers.Add(new GrassRoofRenderer());
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
                var bounds = new RectangleF(
                    (float)(entity.X - 0.7 * entity.Z),
                    (float)(entity.Y - 0.7 * entity.Z),
                    1, 1);

                RenderEntity(canvas, bounds, entity);
            }
        }

        public void RenderEntity(INativeCanvas canvas, RectangleF bounds, EntityBase entity)
        {
            var renderer = _entityRenderers.FirstOrDefault(r => r.CanRender(entity));
            if (renderer != null)
            {
                renderer.Render(canvas, entity, bounds);
            }
        }

        private double GetDistanceToCamera(EntityBase entity)
        {
            return entity.X + entity.Y + entity.Z * 0.7f + entity.GetZIndex();
        }

        public bool Touch(float x, float y, Rectangle viewportBounds)
        {
            return new Raycast(TestHit).Touch(x, y, viewportBounds)
                || _viewModel.ProcessClickOnEntityLayer((int)x, (int)y, null);
        }

        private bool TestHit(Point cell, float z0, float z1)
        {
            foreach(var entity in _viewModel.Stage.GetEntitesInCell(cell.X, cell.Y).OrderByDescending(e => e.Z))
            {
                // z0---z1---e0---e1 -
                // z0---e0---z1---e1 +
                // z0---e0---e1---z1 +
                // e0---z0---e1---z1 +
                // e0---e1---z0---z1 -

                if (entity.UseHitbox() 
                    && z1 >= entity.Z 
                    && entity.Z + entity.GetHeight() >= z0)
                {
                    if (_viewModel.ProcessClickOnEntityLayer(entity.CellX, entity.CellY, entity))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}