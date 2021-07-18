using foxer.Core.Game.Entities;
using foxer.Pages;
using System.Drawing;

namespace foxer.Render
{
    public abstract class EntityRendererBase<TEntity> : IEntityRenderer
        where TEntity : EntityBase
    {
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
