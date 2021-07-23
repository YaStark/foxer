using System;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public abstract class EntityDescriptor<TEntity> : EntityDescriptorBase
        where TEntity : EntityBase
    {
        public sealed override Type EntityType { get; } = typeof(TEntity);

        protected EntityDescriptor(EntityKind kind)
            : base(kind)
        {
        }

        public sealed override ItemBase GetLoot(Stage stage, EntityBase entity)
        {
            return OnGetLoot(stage, entity as TEntity);
        }

        protected virtual ItemBase OnGetLoot(Stage stage, TEntity entity)
        {
            return null;
        }

        protected ItemBase GetRndLoot<TItem>(Stage stage, int min, int max)
            where TItem : ItemBase
        {
            return stage.ItemManager.Create<TItem>(stage, stage.Rnd.Next(min, max));
        }
    }
}
