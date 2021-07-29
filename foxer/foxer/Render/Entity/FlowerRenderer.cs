using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Render
{
    public class FlowerRenderer : EntityRendererBase<FlowerEntity>
    {
        private static readonly byte[] _sprite = Properties.Resources.sprite_flowers;
        private static readonly RectangleF[,] _mask = GeomUtils.CreateUniformSpriteMask(1, 4);

        protected override void Render(INativeCanvas canvas, FlowerEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(_sprite, _mask[0, entity.Kind % 4], bounds);
        }
    }
}
