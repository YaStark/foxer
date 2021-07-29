using System;
using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Core.Game.Items
{
    public abstract class ItemFlowerBase : ItemBase, IBuildableItem
    {
        public int Kind { get; set; }

        public Type EntityType => typeof(FlowerEntity);

        protected ItemFlowerBase(int count)
        {
            Count = count;
        }

        public IPlatform GetTopPlatform(Stage stage, int x, int y)
        {
            return stage.DefaultPlatform;
        }

        public EntityBase Create(Stage stage, int x, int y, float z)
        {
            var flower = new FlowerEntity(x, y, Kind);
            flower.MoveZ(z);
            return flower;
        }

        public EntityBase CreatePreviewItem(int x0, int y0, int x, int y)
        {
            return new FlowerEntity(x, y, Kind);
        }

        public bool CheckCanBuild(Stage stage, int x, int y, float z)
        {
            var entity = Create(stage, x, y, z);
            return stage.CanBePlaced(entity, x, y, stage.DefaultPlatform);
        }

        public bool CheckBuildDistance(Point player, Point target)
        {
            return MathUtils.L2(player, target) < 3;
        }

        public bool UseRotation()
        {
            return false;
        }

        public void RotatePreview(int angle360)
        {
        }
    }

    public class ItemDandelion : ItemFlowerBase
    {
        public ItemDandelion(int count)
            : base(count)
        {
            Count = count;
            Kind = 0;
        }
    }

    public class ItemRedFlower : ItemFlowerBase
    {
        public ItemRedFlower(int count)
            : base(count)
        {
            Count = count;
            Kind = 1;
        }
    }

    public class ItemSunflower : ItemFlowerBase
    {
        public ItemSunflower(int count)
            : base(count)
        {
            Count = count;
            Kind = 2;
        }
    }

    public class ItemBlueFlower : ItemFlowerBase
    {
        public ItemBlueFlower(int count)
            : base(count)
        {
            Count = count;
            Kind = 3;
        }
    }
}
