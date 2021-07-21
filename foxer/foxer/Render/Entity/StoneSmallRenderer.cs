using System;
using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Render
{
    public class StoneSmallRenderer : EntityRendererBase<StoneSmallEntity>
    {
        private static readonly byte[] _sprite = Properties.Resources.sprite_stones_small;
        private static readonly RectangleF[,] _maps = GeomUtils.CreateUniformSpriteMask(1, 3);

        protected override void Render(INativeCanvas canvas, StoneSmallEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(_sprite, GetMap(entity.Kind), bounds);
        }

        private RectangleF GetMap(int kind)
        {
            return _maps[0, Math.Abs(kind) % _maps.Length];
        }
    }
}
