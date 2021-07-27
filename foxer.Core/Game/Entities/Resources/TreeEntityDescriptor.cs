using System;
using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class TreeEntityDescriptor : EntityDescriptor<TreeEntity>
    {
        public TreeEntityDescriptor() 
            : base(EntityKind.BigItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            if (!base.OnCanBePlaced(stage, cell, entites, platform)
                || platform != stage.DefaultPlatform)
            {
                return false;
            }

            return cell.Kind == CellKind.Floor
                || cell.Kind == CellKind.Misc_Tree;
        }

        protected override bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            return descriptor.EntityType == typeof(SquirrelEntity)
                || base.CheckCanOtherBePlacedHere(descriptor);
        }

        protected override ItemBase OnGetLoot(Stage stage, TreeEntity tree)
        {
            const double maxCount = 15;
            if (tree.Age < TreeEntity.AGE_LARGE)
            {
                var stick = stage.ItemManager.Create<ItemStick>(stage);
                stick.Count = (int)(tree.Age * 10 / TreeEntity.AGE_LARGE) + 2;
                return stick;
            }

            double value = Math.Min(1, tree.Age / TreeEntity.AGE_LARGE);
            int count = (int)(maxCount * value * (stage.Rnd.NextDouble() / 4 + 0.75));
            return stage.ItemManager.Create<ItemWood>(stage, count);
        }
    }
}
