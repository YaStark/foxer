using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Render
{
    public class GrassRenderer : EntityRendererBase<GrassEntity>
    {
        private static readonly byte[] _sprite = Properties.Resources.sprite_grass;
        private static readonly RectangleF[,] _mask = GeomUtils.CreateUniformSpriteMask(2, 3);

        protected override void Render(INativeCanvas canvas, GrassEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(
                _sprite, 
                GetMask(entity.Kind, entity.CanGather), 
                ScaleBounds(bounds, 1.3f));
        }

        private RectangleF GetMask(int kind, bool mature)
        {
            int x = mature ? 1 : 0;
            int y = kind % _mask.GetLength(1);
            return _mask[x, y];
        }
    }
}
