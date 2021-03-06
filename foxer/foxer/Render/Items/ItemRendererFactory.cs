using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;

namespace foxer.Render.Items
{
    public class ItemRendererFactory
    {
        private static readonly Dictionary<Type, IItemRenderer> _renderers = new Dictionary<Type, IItemRenderer>();

        static ItemRendererFactory()
        {
            Add(new SimpleItemRenderer<ItemStone>(Properties.Resources.icon_stone));
            Add(new SimpleItemRenderer<ItemStoneAxe>(Properties.Resources.icon_stone_axe));
            Add(new SimpleItemRenderer<ItemWood>(Properties.Resources.icon_wood));
            Add(new SimpleItemRenderer<ItemStick>(Properties.Resources.icon_sticks));
            Add(new SimpleItemRenderer<ItemDigStick>(Properties.Resources.icon_dig_stick));
            Add(new SimpleItemRenderer<ItemStoneOven>(Properties.Resources.icon_stone_oven));
            Add(new SimpleItemRenderer<ItemGrass>(Properties.Resources.icon_grass));
            Add(new SimpleItemRenderer<ItemGrassWall>(Properties.Resources.icon_grass_wall));
            Add(new SimpleItemRenderer<ItemGrassFloor>(Properties.Resources.icon_grass_floor));
            Add(new SimpleItemRenderer<ItemGrassRoof>(Properties.Resources.icon_grass_roof));

            Add(new SimpleItemRenderer<ItemBlueFlower>(Properties.Resources.icon_blue_flower));
            Add(new SimpleItemRenderer<ItemRedFlower>(Properties.Resources.icon_red_flower));
            Add(new SimpleItemRenderer<ItemSunflower>(Properties.Resources.icon_sunflower));
            Add(new SimpleItemRenderer<ItemDandelion>(Properties.Resources.icon_dandelion));
        }

        public IItemRenderer GetRenderer(ItemBase item)
        {
            return _renderers.TryGetValue(item.GetType(), out var renderer)
                ? renderer
                : null;
        }

        public IItemRenderer GetRenderer(Type itemType)
        {
            return _renderers.TryGetValue(itemType, out var renderer)
                ? renderer
                : null;
        }

        private static void Add<T>(ItemRendererBase<T> renderer)
            where T : ItemBase
        {
            _renderers.Add(typeof(T), renderer);
        }
    }
}
