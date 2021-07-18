using foxer.Core.Game.Entities;
using foxer.Render.Helpers;
using System.Drawing;

namespace foxer.Render
{
    public class PlayerRenderer : EntityRendererBase<PlayerEntity>
    {
        private static readonly RotatingAnimationRendererHelper _walking = new RotatingAnimationRendererHelper();
        private static readonly RotatingRendererHelper _standing = new RotatingRendererHelper();

        static PlayerRenderer()
        {
            _walking.AddImagesForRotation(0,
                Properties.Resources.player0_walk_1,
                Properties.Resources.player0_walk_2,
                Properties.Resources.player0_walk_3,
                Properties.Resources.player0);

            _walking.AddImagesForRotation(90,
                Properties.Resources.player90_walk_1,
                Properties.Resources.player90_walk_2,
                Properties.Resources.player90_walk_3,
                Properties.Resources.player90);

            _walking.AddImagesForRotation(180,
                Properties.Resources.player180_walk_1,
                Properties.Resources.player180_walk_2,
                Properties.Resources.player180_walk_3,
                Properties.Resources.player180);

            _walking.AddImagesForRotation(270,
                Properties.Resources.player270_walk_1,
                Properties.Resources.player270_walk_2,
                Properties.Resources.player270_walk_3,
                Properties.Resources.player270);

            _standing.AddImageForRotation(0, Properties.Resources.player0);
            _standing.AddImageForRotation(90, Properties.Resources.player90);
            _standing.AddImageForRotation(180, Properties.Resources.player180);
            _standing.AddImageForRotation(270, Properties.Resources.player270);
        }

        protected override void Render(INativeCanvas canvas, PlayerEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(
                GetImage(entity), 
                ScaleBounds(bounds, 1.5f));
        }

        private byte[] GetImage(PlayerEntity entity)
        {
            if (entity.ActiveAnimation == entity.Walk)
            {
                return _walking.GetImageByRotation(entity.Rotation, entity.Walk);
            }

            return _standing.GetImageByRotation(entity.Rotation);
        }
    }
}
