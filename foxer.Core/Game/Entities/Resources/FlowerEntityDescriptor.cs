using System.Collections.Generic;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Items;

namespace foxer.Core.Game.Entities
{
    public class FlowerEntityDescriptor : EntityDescriptor<FlowerEntity>
    {
        public FlowerEntityDescriptor() 
            : base(EntityKind.SmallItem)
        {
        }

        protected override bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, IPlatform platform)
        {
            return base.OnCanBePlaced(stage, cell, entites, platform)
                && platform == stage.DefaultPlatform
                && cell.Kind == CellKind.Floor;
        }

        protected override ItemBase OnGetLoot(Stage stage, FlowerEntity entity)
        {
            switch(entity.Kind % 4)
            {
                default:
                case 0: return stage.ItemManager.Create<ItemDandelion>(stage, 1);
                case 1: return stage.ItemManager.Create<ItemRedFlower>(stage, 1);
                case 2: return stage.ItemManager.Create<ItemSunflower>(stage, 1);
                case 3: return stage.ItemManager.Create<ItemBlueFlower>(stage, 1);
            }
        }
    }
}
