using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using foxer.Pages;
using System;
using System.Drawing;

namespace foxer.Render
{
    public abstract class EntityRendererBase<TEntity> : IEntityRenderer
        where TEntity : EntityBase
    {
        private static readonly byte[] _hpbar_bg = Properties.Resources.hpbar_background;
        private static readonly byte[] _hpbar_lost = Properties.Resources.hpbar_lost;
        private static readonly byte[] _hpbar_restore = Properties.Resources.hpbar_restore;

        public virtual bool CanRender<T>(T item)
        {
            return item.GetType() == typeof(TEntity);
        }

        public void Render(INativeCanvas canvas, EntityBase entity, RectangleF bounds)
        {
            if (PageGame.EnableDebugGraphics)
            {
                canvas.DrawRectangle(new RectangleF(entity.Cell, new SizeF(1, 1)), Color.Beige);
            }

            Render(canvas, entity as TEntity, bounds);

            if(entity is EntityFighterBase fighter)
            {
                RenderHpBar(canvas, fighter, bounds);
            }
        }

        private void RenderHpBar(INativeCanvas canvas, EntityFighterBase fighter, RectangleF bounds)
        {
            if(fighter.Hitpoints != fighter.MaxHitpoints)
            {
                canvas.Translate(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
                canvas.RotateDegrees(-45);
                var rect = new RectangleF(-0.5f, -fighter.GetHeight() / 3, 1, 0.1f);

                canvas.DrawImage(_hpbar_bg, rect);
                canvas.DrawImage(_hpbar_lost, rect);

                const float pad = 0.07f;
                var width = pad + (1 - pad * 2) * (fighter.Hitpoints / (float)fighter.MaxHitpoints);

                var rect0 = new RectangleF(0, 0, width, 1);
                rect.Width = width;
                canvas.DrawImage(_hpbar_restore, rect0, rect);

                canvas.RotateDegrees(45);
                canvas.Translate(-bounds.Left - bounds.Width / 2, -bounds.Top - bounds.Height / 2);
            }
        }

        protected abstract void Render(INativeCanvas canvas, TEntity entity, RectangleF bounds);

        protected RectangleF ScaleBounds(RectangleF bounds, float scale)
        {
            scale -= 1;
            return RectangleF.FromLTRB(
                bounds.Left - bounds.Width * scale,
                bounds.Top - bounds.Height * scale,
                bounds.Right,
                bounds.Bottom);
        }
    }
}
