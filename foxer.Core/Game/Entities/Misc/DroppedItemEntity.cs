using foxer.Core.Game.Animation;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System;
using System.Drawing;

namespace foxer.Core.Game.Entities
{
    public class DroppedItemEntity : EntityBase
    {
        public ItemBase Item { get; }

        public FollowingAnimation Gather { get; }

        public ZMovingAnimation Idle { get; }

        public DroppedItemEntity(Point cell, ItemBase item) 
            : base(cell.X, cell.Y, 0)
        {
            Item = item;
            Gather = new FollowingAnimation(this, 0.01);
            Idle = new ZMovingAnimation(this, 1600, 0.1f);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            base.OnUpdate(stage, timeMs);
            if(ActiveAnimation == null)
            {
                StartAnimation(Idle.Coroutine);
            }
        }

        public void DoGather(EntityBase gatherer, Func<ItemBase, bool> onCatch)
        {
            Gather.Target = gatherer;
            StartAnimation(
                Gather.Coroutine, 
                GameUtils.DelegateCoroutine(e =>
                {
                    if(onCatch?.Invoke(Item) != false)
                    {
                        BeginDestroy();
                    }
                }));
        }
    }
}
