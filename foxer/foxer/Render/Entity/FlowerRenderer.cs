using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Render
{
    public class FlowerRenderer : EntityRendererBase<FlowerEntity>
    {
        private static readonly byte[][] _imageFlowers;

        static FlowerRenderer()
        {
            _imageFlowers = new[]
            {
                Properties.Resources.flower_1,
                Properties.Resources.flower_2,
                Properties.Resources.flower_3,
                Properties.Resources.flower_4
            };
        }

        protected override void Render(INativeCanvas canvas, FlowerEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(GameUtils.Uniform(_imageFlowers, entity.Kind), bounds);
        }
    }
}
