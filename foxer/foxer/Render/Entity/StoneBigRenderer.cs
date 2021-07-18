using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Render
{
    public class StoneBigRenderer : EntityRendererBase<StoneBigEntity>
    {
        private static readonly byte[][] _images;

        static StoneBigRenderer()
        {
            _images = new[]
            {
                Properties.Resources.stone_big_1,
                Properties.Resources.stone_big_2,
                Properties.Resources.stone_big_3
            };
        }

        protected override void Render(INativeCanvas canvas, StoneBigEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(GameUtils.Uniform(_images, entity.Kind), bounds);
        }
    }
}
