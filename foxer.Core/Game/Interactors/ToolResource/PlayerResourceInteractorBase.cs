using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Interactors
{
    public abstract class PlayerResourceInteractorBase<TResourceEntity> : InteractorBase<PlayerEntity, TResourceEntity>
        where TResourceEntity : EntityBase
    {
        protected virtual IToolItem GetTool(PlayerEntity player)
        {
            return player.Hand as IToolItem;
        }

        protected abstract SimpleAnimation GetToolAnimation(PlayerEntity player);

        protected virtual void OnToolSwipe(PlayerEntity player, TResourceEntity subj, InteractorArgs arg)
        {
            GetToolAnimation(player).DurationMs = GetTool(player).SwipeMs;
            player.SetWorkAggression();
        }

        protected virtual void OnEndInteract(PlayerEntity player, TResourceEntity subj, InteractorArgs arg)
        {
            subj.BeginDestroy();
        }

        protected override bool CanInteract(PlayerEntity player, TResourceEntity obj, InteractorArgs arg)
        {
            return GetTool(player)?.CanInteract(obj) == true;
        }

        protected override bool Interact(PlayerEntity player, TResourceEntity obj, InteractorArgs arg)
        {
            return GetTool(player)?.CanInteract(obj) == true
                && UseTool(player, obj, arg);
        }

        private bool UseTool(PlayerEntity player, TResourceEntity obj, InteractorArgs arg)
        {
            Point[] path = null;
            if (MathUtils.L1(player.Cell, obj.Cell) > 1)
            {
                path = GetPathToItem(player, obj, arg.Stage);
                if(path == null)
                {
                    return false;
                }
            }

            var tool = GetTool(player);
            var toolAnimation = GetToolAnimation(player);
            var swipes = tool.GetSwipesCount(obj);
            if (tool == null || toolAnimation == null || swipes <= 0)
            {
                return false;
            }

            var coroutines = new List<Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>>>();
            coroutines.Add(GameUtils.DelegateCoroutine(x => player.RotateTo(obj.Cell)));
            for (int i = 0; i < swipes; i++)
            {
                coroutines.Add(GameUtils.DelegateCoroutine(x => OnToolSwipe(player, obj, arg)));
                coroutines.Add(toolAnimation.Coroutine);
                coroutines.Add(GameUtils.EnsureCoroutine(x => GetTool(player)?.CanInteract(obj) == true));
            }

            coroutines.Add(GameUtils.DelegateCoroutine(x => OnEndInteract(player, obj, arg)));
            player.MoveThenDo(path, coroutines.ToArray());
            return true;
        }
    }
}
