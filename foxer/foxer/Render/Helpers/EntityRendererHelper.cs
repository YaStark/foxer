using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Render.Helpers
{
    public class EntityRendererHelper<TEntity>
        where TEntity : EntityBase
    {
        private class RendererHelperItem
        {
            private readonly Func<TEntity, bool> _selector;
            public IRendererHelper RendererHelper { get; }

            public RendererHelperItem(Func<TEntity, bool> selector, IRendererHelper rendererHelper)
            {
                _selector = selector;
                RendererHelper = rendererHelper;
            }

            public bool CanRender(TEntity entity)
            {
                return _selector(entity);
            }
        }

        private readonly List<RendererHelperItem> _items = new List<RendererHelperItem>();
        private IRendererHelper _defaultItem;

        public EntityRendererHelper<TEntity> AddAnimation(Func<TEntity, bool> selector, IRendererHelper rendererHelper)
        {
            _items.Add(new RendererHelperItem(selector, rendererHelper));
            return this;
        }

        public EntityRendererHelper<TEntity> AddRotatingAnimation(Func<TEntity, bool> selector, byte[] sprite, int width, int height, bool reverse = false)
        {
            _items.Add(new RendererHelperItem(
                selector, 
                new RotatingAnimationSpriteRendererHelper(sprite, width, height)
                {
                    Reverse = reverse
                }));
            return this;
        }

        public EntityRendererHelper<TEntity> UseAsDefault()
        {
            _defaultItem = _items.Last()?.RendererHelper;
            return this;
        }
        
        public void Render(INativeCanvas canvas, RectangleF bounds, TEntity entity)
        {
            if(entity.ActiveAnimation == null)
            {
                _defaultItem?.Render(canvas, bounds, entity);
                return;
            }

            (_items.FirstOrDefault(i => i.CanRender(entity))?.RendererHelper ?? _defaultItem)
                ?.Render(canvas, bounds, entity);
        }
    }
}
