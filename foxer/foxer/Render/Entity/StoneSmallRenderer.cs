using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Render
{
    public class StoneSmallRenderer : EntityRendererBase<StoneSmallEntity>
    {
        private static readonly byte[][] _images;

        static StoneSmallRenderer()
        {
            _images = new[]
            {
                Properties.Resources.stone_small_1,
                Properties.Resources.stone_small_2,
                Properties.Resources.stone_small_3
            };
        }

        protected override void Render(INativeCanvas canvas, StoneSmallEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(GameUtils.Uniform(_images, entity.Kind), bounds);
        }
    }
}
