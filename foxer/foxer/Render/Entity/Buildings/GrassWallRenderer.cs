using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class GrassWallRenderer : EntityRendererBase<GrassWallEntity>
    {
        private static readonly RotatingSpriteRendererHelper _wall = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_wall, 4);

        private static readonly RotatingSpriteRendererHelper _window = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_window, 4);

        private static readonly RotatingSpriteRendererHelper _door = new RotatingSpriteRendererHelper(
            Properties.Resources.sprite_grass_door, 4);

        protected override void Render(INativeCanvas canvas, GrassWallEntity entity, RectangleF bounds)
        {
            bounds = GeomUtils.Deflate(bounds, -bounds.Width, -bounds.Height); // 3x size
            switch(entity.WallKind)
            {
                case WallKind.Wall:
                    _wall.Render(canvas, bounds, entity.Rotation);
                    break;

                case WallKind.Window:
                    _window.Render(canvas, bounds, entity.Rotation);
                    break;

                case WallKind.Door:
                    _door.Render(canvas, bounds, entity.Rotation);
                    break;
            }
        }
    }
}
