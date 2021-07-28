using System;
using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Core.Utils;

namespace foxer.Core.Game.Items
{
    public abstract class BuildableItemBase<TEntity> : ItemBase, IBuildableItem
        where TEntity : EntityBase
    {
        public Type EntityType => typeof(TEntity);

        public int Rotation { get; private set; }

        public bool CheckBuildDistance(Point player, Point target)
        {
            return MathUtils.L2(player, target) < 2;
        }

        public bool CheckCanBuild(Stage stage, int x, int y, float z)
        {
            var entity = CreatePreviewItem(stage.ActiveEntity.CellX, stage.ActiveEntity.CellY, x, y);
            var platform = stage.GetLowerPlatform(entity, x, y, z + 0.01f);
            return stage.CanBePlaced(entity, x, y, platform)
                && OnCheckCanBuild(stage, x, y, platform);
        }

        protected virtual bool OnCheckCanBuild(Stage stage, int x, int y, IPlatform platform)
        {
            return true;
        }

        public EntityBase Create(Stage stage, int x, int y, float z)
        {
            var entity = CreateEntity(stage.ActiveEntity.CellX, stage.ActiveEntity.CellY, x, y, false);
            entity.MoveZ(z);
            return entity;
        }

        public EntityBase CreatePreviewItem(int x0, int y0, int x, int y)
        {
            return CreateEntity(x0, y0, x, y, true);
        }

        public IPlatform GetTopPlatform(Stage stage, int x, int y)
        {
            return stage.GetTopPlatform(
                CreatePreviewItem(stage.ActiveEntity.CellX, stage.ActiveEntity.CellY, x, y),
                x, y);
        }

        public void RotatePreview(int angle360)
        {
            Rotation = angle360;
        }

        public virtual bool UseRotation()
        {
            return true;
        }

        protected abstract EntityBase CreateEntity(int x0, int y0, int x, int y, bool preview);
    }
}
